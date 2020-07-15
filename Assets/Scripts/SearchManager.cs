using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SearchManager : MonoBehaviour {

    // 0 = 배우, 1 = 공연, 2 = 극장, 3 = 제작자, 4 = 제작사
    int cate;
    string search;

    // search key(ex "actor_id"), search keyword(ex "김대종")
    Dictionary<string, string> searchKey = new Dictionary<string, string>();
    // result(ex "룽게"), importance(1~5)
    Dictionary<string, int> searchResult = new Dictionary<string, int>();

    List<Text> searchText = new List<Text>();
    List<Text> resultText = new List<Text>();

    [SerializeField] Dropdown _category;
    [SerializeField] InputField _searchBar;
    [SerializeField] Text _keyword;
    [SerializeField] GameObject _popup;
    [SerializeField] Text _searchsearch;

    Transform popup;

    private void Start()
    {
        _category = _category.GetComponent<Dropdown>();
        _searchBar = _searchBar.GetComponent<InputField>();
        _keyword = _keyword.GetComponent<Text>();
        popup = _popup.GetComponent<Transform>();
        _searchsearch = _searchsearch.GetComponent<Text>();
    }

    public void AddKeyword()
    {
        cate = _category.value;
        search = _searchBar.text;
        switch (cate)
        {
            case 0:
                var temp0 = from a in DB.Data().Actors
                        where a.Actor_name == search
                        select a.Actor_id;
                foreach (var result in temp0)
                {
                    searchKey.Add("actor_id", result);
                    _keyword.text = _keyword.text + " " + search;
                }
                break;
            case 1:
                var temp1 = from s in DB.Data().Stages
                           where s.Stage_name == search
                           select s.Stage_id;
                foreach (var result in temp1)
                {
                    searchKey.Add("stage_id", result);
                    _keyword.text = _keyword.text + " " + search;
                }
                break;
            case 2:
                var temp2 = from t in DB.Data().Theaters
                           where t.Theater_name == search
                           select t.Theater_name;
                foreach (var result in temp2)
                {
                    searchKey.Add("theater_name", result);
                    _keyword.text = _keyword.text + " " + search;
                }
                break;
            case 3:
                var temp3 = from c in DB.Data().Creators
                            where c.Creator_name == search
                            select c.Creator_id;
                foreach (var result in temp3)
                {
                    searchKey.Add("creator_id", result);
                    _keyword.text = _keyword.text + " " + search;
                }
                break;
            case 4:
                var temp4 = from c in DB.Data().Companies
                            where c.Company_name == search
                            select c.Company_name;
                foreach (var result in temp4)
                {
                    searchKey.Add("company_name", result);
                    _keyword.text = _keyword.text + " " + search;
                }
                break;
        }

        
    }

    public void Search() {
        // 릴레이션이 있다면 그걸 가장 먼저/중요한 결과로 +3
        // ACT
        if (searchKey.ContainsKey("actor_id") && searchKey.ContainsKey("stage_id"))
        {
            var temp1 = from a in DB.Data().Acts
                        where a.Act_actor_id == searchKey["actor_id"]
                        && a.Act_stage_id == searchKey["stage_id"]
                        select a.Act_cast;
            foreach (var result in temp1)
            {
                searchResult.Add(result, 3);
            }
        }
        // PLAY
        else if (searchKey.ContainsKey("stage_id") && searchKey.ContainsKey("theater_name"))
        {
            var temp2 = from p in DB.Data().Plays
                        where p.Play_stage_id == searchKey["stage_id"]
                        && p.Play_theater_name == searchKey["theater_name"]
                        select p.Play_loc;
            foreach (var result in temp2)
            {
                searchResult.Add(result, 3);
            }
        }
        
        // 그 외엔 각 키를 포함하는 릴레이션을 순회하며 하나씩 추가. 발견할 때마다 중요도도 +1
        // actor_id / stage_id / theater_name / creator_name / company_name
        if (searchKey.ContainsKey("actor_id"))
        {
            var tempActor = from a in DB.Data().Actors
                            where a.Actor_id == searchKey["actor_id"]
                            select a.Actor_year;
            foreach (var ee in tempActor)
                AddResult(ee);

            var tempActor2 = from a in DB.Data().Acts
                             where a.Act_actor_id == searchKey["actor_id"]
                             join s in DB.Data().Stages on a.Act_stage_id equals s.Stage_id
                             select new { cast = a.Act_cast, stage = s.Stage_name };
            foreach (var ee in tempActor2)
            {
                AddResult(ee.cast);
                AddResult(ee.stage);
            }
        }
        if (searchKey.ContainsKey("stage_id"))
        {
            var tempStage = from a in DB.Data().Acts
                            where a.Act_stage_id == searchKey["stage_id"]
                            join aa in DB.Data().Actors on a.Act_actor_id equals aa.Actor_id
                            select new { cast = a.Act_cast, actor = aa.Actor_name };
            foreach (var ee in tempStage)
            {
                AddResult(ee.cast);
                AddResult(ee.actor);
            }

            var tempStage2 = from c in DB.Data().Contributes
                             where c.Contribute_stage_id == searchKey["stage_id"]
                             join com in DB.Data().Companies on c.Contribute_company_name equals com.Company_name
                             select new { company = c.Contribute_company_name, year = com.Company_year };
            foreach (var ee in tempStage2)
                AddResult(ee.company);

            var tempStage3 = from s in DB.Data().Stages
                             where s.Stage_id == searchKey["stage_id"]
                             select s;
            foreach (var ee in tempStage3)
            {
                AddResult(ee.Stage_season);
                AddResult(ee.Stage_year);
            }

            var tempStage4 = from m in DB.Data().Makes
                             where m.Make_stage_id == searchKey["stage_id"]
                             join cre in DB.Data().Creators on m.Make_creator_id equals cre.Creator_id
                             select cre.Creator_name;
            foreach (var ee in tempStage4)
                AddResult(ee);

            var tempStage5 = from p in DB.Data().Plays
                             where p.Play_stage_id == searchKey["stage_id"]
                             join the in DB.Data().Theaters on p.Play_theater_name equals the.Theater_name
                             select new { loc = p.Play_loc, location = the.Theater_location };
            foreach (var ee in tempStage5)
            {
                AddResult(ee.loc);
                AddResult(ee.location);
            }
        }
        if (searchKey.ContainsKey("theater_name"))
        {
            var tempThe = from p in DB.Data().Plays
                          where p.Play_theater_name == searchKey["theater_name"]
                          join s in DB.Data().Stages on p.Play_stage_id equals s.Stage_id
                          select new { loc = p.Play_loc, stage = s.Stage_name };
            foreach (var ee in tempThe)
            {
                AddResult(ee.loc);
                AddResult(ee.stage);
            }

            var tempThe2 = from t in DB.Data().Theaters
                           where t.Theater_name == searchKey["theater_name"]
                           select t.Theater_location;
            foreach (var ee in tempThe2)
                AddResult(ee);

        }
        if (searchKey.ContainsKey("creator_id"))
        {
            var tempCreator = from m in DB.Data().Makes
                              where m.Make_creator_id == searchKey["creator_id"]
                              join sta in DB.Data().Stages on m.Make_stage_id equals sta.Stage_id
                              select sta.Stage_name;
            foreach (var ee in tempCreator)
                AddResult(ee);
        }
        if (searchKey.ContainsKey("company_name"))
        {
            var tempCompany = from c in DB.Data().Companies
                              where c.Company_name == searchKey["company_name"]
                              select c.Company_year;
            foreach (var ee in tempCompany)
                AddResult(ee);

            var tempCompany2 = from c in DB.Data().Contributes
                               where c.Contribute_company_name == searchKey["company_name"]
                               join sta in DB.Data().Stages on c.Contribute_stage_id equals sta.Stage_id
                               select sta.Stage_name;
            foreach (var ee in tempCompany2)
                AddResult(ee);
        }

        DrawPopup();
    }

    private void DrawPopup()
    {
        int idx = 0;

        foreach (var result in searchResult)
        {
            searchText.Add(_searchsearch);
            resultText.Add(_searchsearch);
        }

        foreach (var result in searchResult)
        {
            searchText[idx].text = result.Key;
            searchText[idx].fontSize = 30 + result.Value * result.Value;
            searchText[idx].color = new Color(0.0f, 0.0f, 0.0f, 0.12f * result.Value);

            resultText[idx] = Instantiate(searchText[idx], new Vector3(Random.Range(100.0f, 900.0f), Random.Range(100.0f, 600.0f), 0), Quaternion.identity) as Text;
            resultText[idx].transform.SetParent(popup);
            
            idx++;
        }
    }

    private void AddResult(string s)
    {
        if (searchResult.ContainsKey(s))
            searchResult[s] += 1;
        else
            searchResult.Add(s, 1);
    }

    public void Clear()
    {
        searchKey.Clear();
        searchResult.Clear();

        foreach (var r in resultText)
            Destroy(r);

        searchText.Clear();
        resultText.Clear();
        _keyword.text = "";
    }

}
