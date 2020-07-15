using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class Actor
{
    private string actor_id;
    private string actor_name;
    private string actor_year;
    public string Actor_id
    {
        get
        {
            return actor_id;
        }

        set
        {
            actor_id = value;
        }
    }

    public string Actor_name
    {
        get
        {
            return actor_name;
        }

        set
        {
            actor_name = value;
        }
    }

    public string Actor_year
    {
        get
        {
            return actor_year;
        }

        set
        {
            actor_year = value;
        }
    }
}

public class Act
{
    private string act_cast;
    private string act_actor_id;
    private string act_stage_id;

    public string Act_cast
    {
        get
        {
            return act_cast;
        }

        set
        {
            act_cast = value;
        }
    }

    public string Act_actor_id
    {
        get

        {
            return act_actor_id;
        }

        set
        {
            act_actor_id = value;
        }
    }

    public string Act_stage_id
    {
        get
        {
            return act_stage_id;
        }

        set
        {
            act_stage_id = value;
        }
    }
}

public class Company
{
    private string company_name;
    private string company_year;

    public string Company_name
    {
        get
        {
            return company_name;
        }

        set
        {
            company_name = value;
        }
    }

    public string Company_year
    {
        get
        {
            return company_year;
        }

        set
        {
            company_year = value;
        }
    }
}

public class Contribute
{
    private string contribute_stage_id;
    private string contribute_company_name;

    public string Contribute_stage_id
    {
        get
        {
            return contribute_stage_id;
        }

        set
        {
            contribute_stage_id = value;
        }
    }

    public string Contribute_company_name
    {
        get
        {
            return contribute_company_name;
        }

        set
        {
            contribute_company_name = value;
        }
    }
}

public class Creator
{
    private string creator_id;
    private string creator_name;

    public string Creator_id
    {
        get
        {
            return creator_id;
        }

        set
        {
            creator_id = value;
        }
    }

    public string Creator_name
    {
        get
        {
            return creator_name;
        }

        set
        {
            creator_name = value;
        }
    }
}

public class Make
{
    private string make_stage_id;
    private string make_creator_id;

    public string Make_stage_id
    {
        get
        {
            return make_stage_id;
        }

        set
        {
            make_stage_id = value;
        }
    }

    public string Make_creator_id
    {
        get
        {
            return make_creator_id;
        }

        set
        {
            make_creator_id = value;
        }
    }
}

public class Play
{
    private string play_loc;
    private string play_stage_id;
    private string play_theater_name;

    public string Play_loc
    {
        get
        {
            return play_loc;
        }

        set
        {
            play_loc = value;
        }
    }

    public string Play_stage_id
    {
        get
        {
            return play_stage_id;
        }

        set
        {
            play_stage_id = value;
        }
    }

    public string Play_theater_name
    {
        get
        {
            return play_theater_name;
        }

        set
        {
            play_theater_name = value;
        }
    }
}

public class Stage
{
    private string stage_id;
    private string stage_name;
    private string stage_year;
    private string stage_season;

    public string Stage_id
    {
        get
        {
            return stage_id;
        }

        set
        {
            stage_id = value;
        }
    }

    public string Stage_name
    {
        get
        {
            return stage_name;
        }

        set
        {
            stage_name = value;
        }
    }

    public string Stage_year
    {
        get
        {
            return stage_year;
        }

        set
        {
            stage_year = value;
        }
    }

    public string Stage_season
    {
        get
        {
            return stage_season;
        }

        set
        {
            stage_season = value;
        }
    }
}

public class Theater
{
    private string theater_name;
    private string theater_location;

    public string Theater_name
    {
        get
        {
            return theater_name;
        }

        set
        {
            theater_name = value;
        }
    }

    public string Theater_location
    {
        get
        {
            return theater_location;
        }

        set
        {
            theater_location = value;
        }
    }
}

public class DB
{
    private static DB db = null;
    protected DB() { }
    public static DB Data()
    {
        if (db == null)
            db = new DB();
        return db;
    }

    public List<Actor> Actors;
    public List<Act> Acts;
    public List<Company> Companies;
    public List<Contribute> Contributes;
    public List<Creator> Creators;
    public List<Make> Makes;
    public List<Play> Plays;
    public List<Stage> Stages;
    public List<Theater> Theaters;
}

public class DBManager : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        XElement xActor = XElement.Load("Assets/Resources/Database/ACTOR.xml");
        XElement xAct = XElement.Load("Assets/Resources/Database/ACT.xml");
        XElement xCompany = XElement.Load("Assets/Resources/Database/COMPANY.xml");
        XElement xContribute = XElement.Load("Assets/Resources/Database/CONTRIBUTE.xml");
        XElement xCreator = XElement.Load("Assets/Resources/Database/CREATOR.xml");
        XElement xMake = XElement.Load("Assets/Resources/Database/MAKE.xml");
        XElement xPlay = XElement.Load("Assets/Resources/Database/PLAY.xml");
        XElement xStage = XElement.Load("Assets/Resources/Database/STAGE.xml");
        XElement xTheator = XElement.Load("Assets/Resources/Database/THEATER.xml");
        
        List<Actor> actorList = new List<Actor>();
        int three = 0;
        int idx = -1;
        foreach (var actor in xActor.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    actorList.Add(new Actor());
                    idx++;
                    actorList[idx].Actor_id = TrimCDATA(actor.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    actorList[idx].Actor_name = TrimCDATA(actor.FirstNode.ToString());
                    three++;
                    break;
                case 2:
                    actorList[idx].Actor_year = TrimCDATA(actor.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Act> actList = new List<Act>();
        three = 0;
        idx = -1;
        foreach (var i in xAct.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    actList.Add(new Act());
                    idx++;
                    actList[idx].Act_cast = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    actList[idx].Act_actor_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 2:
                    actList[idx].Act_stage_id = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Company> companyList = new List<Company>();
        three = 0;
        idx = -1;
        foreach (var i in xCompany.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    companyList.Add(new Company());
                    idx++;
                    companyList[idx].Company_name = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    companyList[idx].Company_year = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Contribute> contributeList = new List<Contribute>();
        three = 0;
        idx = -1;
        foreach (var i in xContribute.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    contributeList.Add(new Contribute());
                    idx++;
                    contributeList[idx].Contribute_stage_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    contributeList[idx].Contribute_company_name = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Creator> creatorList = new List<Creator>();
        three = 0;
        idx = -1;
        foreach (var i in xCreator.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    creatorList.Add(new Creator());
                    idx++;
                    creatorList[idx].Creator_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    creatorList[idx].Creator_name = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Make> makeList = new List<Make>();
        three = 0;
        idx = -1;
        foreach (var i in xMake.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    makeList.Add(new Make());
                    idx++;
                    makeList[idx].Make_stage_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    makeList[idx].Make_creator_id = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Play> playList = new List<Play>();
        three = 0;
        idx = -1;
        foreach (var i in xPlay.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    playList.Add(new Play());
                    idx++;
                    playList[idx].Play_loc = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    playList[idx].Play_stage_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 2:
                    playList[idx].Play_theater_name = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Stage> stageList = new List<Stage>();
        three = 0;
        idx = -1;
        foreach (var i in xStage.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    stageList.Add(new Stage());
                    idx++;
                    stageList[idx].Stage_id = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    stageList[idx].Stage_name = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 2:
                    stageList[idx].Stage_year = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 3:
                    stageList[idx].Stage_season = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }

        List<Theater> theaterList = new List<Theater>();
        three = 0;
        idx = -1;
        foreach (var i in xTheator.Elements("ROW").Elements("COLUMN"))
        {
            switch (three)
            {
                case 0:
                    theaterList.Add(new Theater());
                    idx++;
                    theaterList[idx].Theater_name = TrimCDATA(i.FirstNode.ToString());
                    three++;
                    break;
                case 1:
                    theaterList[idx].Theater_location = TrimCDATA(i.FirstNode.ToString());
                    three = 0;
                    break;
            }
        }


        DB.Data().Actors = new List<Actor>(actorList);
        DB.Data().Acts = new List<Act>(actList);
        DB.Data().Companies = new List<Company>(companyList);
        DB.Data().Contributes = new List<Contribute>(contributeList);
        DB.Data().Creators = new List<Creator>(creatorList);
        DB.Data().Makes = new List<Make>(makeList);
        DB.Data().Plays = new List<Play>(playList);
        DB.Data().Stages = new List<Stage>(stageList);
        DB.Data().Theaters = new List<Theater>(theaterList);

}

    public string TrimCDATA(string s) {
        int start = s.IndexOf("D") + 5;
        int end = s.IndexOf("]") - 1;
        s = s.Substring(start, end - start + 1);
        return s;
    }
}
