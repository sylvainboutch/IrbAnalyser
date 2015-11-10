﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrbAnalyser
{
    public static class SpecialStudys
    {
        public static readonly List<irbpair> closedStudys = new List<irbpair>()
        {
            new irbpair("brany", "11-06-234-01"),
            new irbpair("brany", "11-06-242-01"),
            new irbpair("brany", "11-06-262-01"),
            new irbpair("brany", "11-06-287-01"),
            new irbpair("brany", "11-06-318-01"),
            new irbpair("brany", "11-06-95-01"),
            new irbpair("brany", "12-06-130-01"),
            new irbpair("brany", "12-06-179-01"),
            new irbpair("brany", "12-06-193-01"),
            new irbpair("brany", "12-06-218-01"),
            new irbpair("brany", "12-06-30-01"),
            new irbpair("brany", "12-06-320-01"),
            new irbpair("brany", "12-06-324-01"),
            new irbpair("brany", "12-06-341-01"),
            new irbpair("brany", "12-06-399-01"),
            new irbpair("brany", "12-10-23-01"),
            new irbpair("brany", "13-02-171-01"),
            new irbpair("brany", "13-06-110-01"),
            new irbpair("brany", "13-06-142-01"),
            new irbpair("brany", "13-06-145-01"),
            new irbpair("brany", "13-06-193-01"),
            new irbpair("brany", "13-06-286-01"),
            new irbpair("brany", "13-06-297-01"),
            new irbpair("brany", "13-06-298-01"),
            new irbpair("brany", "13-06-328Q-01"),
            new irbpair("brany", "13-06-62-01"),
            new irbpair("brany", "13-06-79Q-01"),
            new irbpair("brany", "14-06-258Q-01"),
            new irbpair("brany", "14-06-314-01"),
            new irbpair("brany", "14-06-340Q-01"),
            new irbpair("brany", "14-06-361-01"),
            new irbpair("brany", "15-02-90-01"),
            new irbpair("brany", "15-06-121-01"),
            new irbpair("brany", "15-06-149-01"),
            new irbpair("brany", "15-06-161-01"),
            new irbpair("brany", "15-06-187-01"),
            new irbpair("brany", "15-06-201-01"),
            new irbpair("brany", "15-06-229-01"),
            new irbpair("brany", "15-06-302-01"),
            new irbpair("brany", "15-06-69-01"),
            new irbpair("brany", "15-06-91-01"),
            new irbpair("brany", "15-11-188-01"),
            new irbpair("einstein", "02-11-290"),
            new irbpair("einstein", "02-12-328E"),
            new irbpair("einstein", "03-03-065S"),
            new irbpair("einstein", "03-03-075"),
            new irbpair("einstein", "03-11-290S"),
            new irbpair("einstein", "04-02-027S"),
            new irbpair("einstein", "04-02-028S"),
            new irbpair("einstein", "04-03-061"),
            new irbpair("einstein", "04-03-062"),
            new irbpair("einstein", "04-04-093"),
            new irbpair("einstein", "04-04-094"),
            new irbpair("einstein", "04-04-101"),
            new irbpair("einstein", "04-05-129S"),
            new irbpair("einstein", "04-11-310"),
            new irbpair("einstein", "05-05-115"),
            new irbpair("einstein", "05-05-116"),
            new irbpair("einstein", "05-10-277S"),
            new irbpair("einstein", "05-10-278S"),
            new irbpair("einstein", "06-01-006S"),
            new irbpair("einstein", "06-02-043S"),
            new irbpair("einstein", "06-06-316"),
            new irbpair("einstein", "06-07-335E"),
            new irbpair("einstein", "06-08-367"),
            new irbpair("einstein", "06-12-487"),
            new irbpair("einstein", "07-03-075M"),
            new irbpair("einstein", "07-03-080S"),
            new irbpair("einstein", "07-08-282E"),
            new irbpair("einstein", "07-10-374"),
            new irbpair("einstein", "08-01-017"),
            new irbpair("einstein", "08-01-018S"),
            new irbpair("einstein", "08-03-068E"),
            new irbpair("einstein", "08-05-137"),
            new irbpair("einstein", "08-06-200"),
            new irbpair("einstein", "08-06-208E"),
            new irbpair("einstein", "08-07-237"),
            new irbpair("einstein", "08-08-276"),
            new irbpair("einstein", "08-09-328E"),
            new irbpair("einstein", "08-10-350"),
            new irbpair("einstein", "08-11-389"),
            new irbpair("einstein", "09-03-079"),
            new irbpair("einstein", "09-05-158E"),
            new irbpair("einstein", "09-07-201"),
            new irbpair("einstein", "09-08-242"),
            new irbpair("einstein", "09-10-304"),
            new irbpair("einstein", "09-10-318"),
            new irbpair("einstein", "09-11-355"),
            new irbpair("einstein", "09-11-356"),
            new irbpair("einstein", "09-11-358"),
            new irbpair("einstein", "10-02-034"),
            new irbpair("einstein", "10-03-055"),
            new irbpair("einstein", "10-03-057"),
            new irbpair("einstein", "10-04-088"),
            new irbpair("einstein", "10-04-096"),
            new irbpair("einstein", "10-04-097"),
            new irbpair("einstein", "10-04-105E"),
            new irbpair("einstein", "10-05-136E"),
            new irbpair("einstein", "10-07-207"),
            new irbpair("einstein", "10-09-292"),
            new irbpair("einstein", "10-10-338E"),
            new irbpair("einstein", "10-10-339E"),
            new irbpair("einstein", "10-11-344"),
            new irbpair("einstein", "10-12-386"),
            new irbpair("einstein", "11-02-073"),
            new irbpair("einstein", "11-03-109"),
            new irbpair("einstein", "11-04-147"),
            new irbpair("einstein", "11-06-230"),
            new irbpair("einstein", "11-06-231"),
            new irbpair("einstein", "11-06-232"),
            new irbpair("einstein", "11-06-235E"),
            new irbpair("einstein", "11-09-342E"),
            new irbpair("einstein", "11-10-352E"),
            new irbpair("einstein", "11-12-410E"),
            new irbpair("einstein", "11-12-434"),
            new irbpair("einstein", "1199405191"),
            new irbpair("einstein", "1199406246"),
            new irbpair("einstein", "1199509353"),
            new irbpair("einstein", "1199511455"),
            new irbpair("einstein", "1199712472"),
            new irbpair("einstein", "1199806145"),
            new irbpair("einstein", "1199905158"),
            new irbpair("einstein", "1200001012"),
            new irbpair("einstein", "1200002058"),
            new irbpair("einstein", "1200005129"),
            new irbpair("einstein", "1200005130"),
            new irbpair("einstein", "1200007210"),
            new irbpair("einstein", "1200009297"),
            new irbpair("einstein", "1200011350"),
            new irbpair("einstein", "1200011351"),
            new irbpair("einstein", "1200101414"),
            new irbpair("einstein", "1200104073"),
            new irbpair("einstein", "12-02-062E"),
            new irbpair("einstein", "12-03-072"),
            new irbpair("einstein", "12-05-196E"),
            new irbpair("einstein", "12-05-212"),
            new irbpair("einstein", "12-05-213"),
            new irbpair("einstein", "12-09-295"),
            new irbpair("einstein", "12-09-297"),
            new irbpair("einstein", "12-09-307"),
            new irbpair("einstein", "12-09-344"),
            new irbpair("einstein", "12-10-357"),
            new irbpair("einstein", "12-10-363"),
            new irbpair("einstein", "13-01-019"),
            new irbpair("einstein", "13-01-020"),
            new irbpair("einstein", "13-01-048"),
            new irbpair("einstein", "13-02-061"),
            new irbpair("einstein", "13-05-115"),
            new irbpair("einstein", "13-05-118"),
            new irbpair("einstein", "13-06-123"),
            new irbpair("einstein", "1995-189"),
            new irbpair("einstein", "1998-106"),
            new irbpair("einstein", "1999-075"),
            new irbpair("einstein", "1999-183"),
            new irbpair("einstein", "1999-184"),
            new irbpair("einstein", "1999-240"),
            new irbpair("einstein", "2000-007"),
            new irbpair("einstein", "2002-214"),
            new irbpair("einstein", "2003-003"),
            new irbpair("einstein", "2003-028"),
            new irbpair("einstein", "2003-127"),
            new irbpair("einstein", "2003-412"),
            new irbpair("einstein", "2003-434"),
            new irbpair("einstein", "2004-047"),
            new irbpair("einstein", "2004-064"),
            new irbpair("einstein", "2004-128"),
            new irbpair("einstein", "2004-667"),
            new irbpair("einstein", "2005-405"),
            new irbpair("einstein", "2005-406"),
            new irbpair("einstein", "2005-528"),
            new irbpair("einstein", "2005-536"),
            new irbpair("einstein", "2005-677"),
            new irbpair("einstein", "2006-271"),
            new irbpair("einstein", "2006-279"),
            new irbpair("einstein", "2006-318"),
            new irbpair("einstein", "2006-484"),
            new irbpair("einstein", "2007-304"),
            new irbpair("einstein", "2007-317"),
            new irbpair("einstein", "2007-405"),
            new irbpair("einstein", "2007-418"),
            new irbpair("einstein", "2007-552"),
            new irbpair("einstein", "2007-554"),
            new irbpair("einstein", "2007-579"),
            new irbpair("einstein", "2007-587"),
            new irbpair("einstein", "2007-607"),
            new irbpair("einstein", "2008-230"),
            new irbpair("einstein", "2008-287"),
            new irbpair("einstein", "2008-314"),
            new irbpair("einstein", "2008-359"),
            new irbpair("einstein", "2008-396"),
            new irbpair("einstein", "2008-411"),
            new irbpair("einstein", "2008-423"),
            new irbpair("einstein", "2008-492"),
            new irbpair("einstein", "2008-493"),
            new irbpair("einstein", "2008-525"),
            new irbpair("einstein", "2008-526"),
            new irbpair("einstein", "2008-534"),
            new irbpair("einstein", "2009-236"),
            new irbpair("einstein", "2009-255"),
            new irbpair("einstein", "2009-272"),
            new irbpair("einstein", "2009-274"),
            new irbpair("einstein", "2009-279"),
            new irbpair("einstein", "2009-406"),
            new irbpair("einstein", "2009-455"),
            new irbpair("einstein", "2009-500"),
            new irbpair("einstein", "2010-276"),
            new irbpair("einstein", "2010-311"),
            new irbpair("einstein", "2010-326"),
            new irbpair("einstein", "2010-349"),
            new irbpair("einstein", "2010-366"),
            new irbpair("einstein", "2010-379"),
            new irbpair("einstein", "2010-400"),
            new irbpair("einstein", "2010-446"),
            new irbpair("einstein", "2010-459"),
            new irbpair("einstein", "2010-562"),
            new irbpair("einstein", "2010-567"),
            new irbpair("einstein", "2010-577"),
            new irbpair("einstein", "2011-362"),
            new irbpair("einstein", "2011-387"),
            new irbpair("einstein", "2011-402"),
            new irbpair("einstein", "2011-421"),
            new irbpair("einstein", "2011-552"),
            new irbpair("einstein", "2011-559"),
            new irbpair("einstein", "2011-561"),
            new irbpair("einstein", "2011-629"),
            new irbpair("einstein", "2012-278"),
            new irbpair("einstein", "2012-310"),
            new irbpair("einstein", "2012-361"),
            new irbpair("einstein", "2012-426"),
            new irbpair("einstein", "2012-436"),
            new irbpair("einstein", "2012-444"),
            new irbpair("einstein", "2012-547"),
            new irbpair("einstein", "2012-680"),
            new irbpair("einstein", "2013-2010"),
            new irbpair("einstein", "2013-2036"),
            new irbpair("einstein", "2013-2038"),
            new irbpair("einstein", "2013-2067"),
            new irbpair("einstein", "2013-2082"),
            new irbpair("einstein", "2013-2089"),
            new irbpair("einstein", "2013-2116"),
            new irbpair("einstein", "2013-2118"),
            new irbpair("einstein", "2013-2133"),
            new irbpair("einstein", "2013-2148"),
            new irbpair("einstein", "2013-2150"),
            new irbpair("einstein", "2013-2159"),
            new irbpair("einstein", "2013-2161"),
            new irbpair("einstein", "2013-2167"),
            new irbpair("einstein", "2013-2171"),
            new irbpair("einstein", "2013-2212"),
            new irbpair("einstein", "2013-2216"),
            new irbpair("einstein", "2013-2232"),
            new irbpair("einstein", "2013-2243"),
            new irbpair("einstein", "2013-2292"),
            new irbpair("einstein", "2013-2306"),
            new irbpair("einstein", "2013-2336"),
            new irbpair("einstein", "2013-2350"),
            new irbpair("einstein", "2013-2353"),
            new irbpair("einstein", "2013-2355"),
            new irbpair("einstein", "2013-2369"),
            new irbpair("einstein", "2013-2370"),
            new irbpair("einstein", "2013-2383"),
            new irbpair("einstein", "2013-2393"),
            new irbpair("einstein", "2013-2395"),
            new irbpair("einstein", "2013-2396"),
            new irbpair("einstein", "2013-2407"),
            new irbpair("einstein", "2013-2431"),
            new irbpair("einstein", "2013-2457"),
            new irbpair("einstein", "2013-248"),
            new irbpair("einstein", "2013-2483"),
            new irbpair("einstein", "2013-2499"),
            new irbpair("einstein", "2013-2507"),
            new irbpair("einstein", "2013-2516"),
            new irbpair("einstein", "2013-2531"),
            new irbpair("einstein", "2013-2532"),
            new irbpair("einstein", "2013-2557"),
            new irbpair("einstein", "2013-2570"),
            new irbpair("einstein", "2013-2585"),
            new irbpair("einstein", "2013-2597"),
            new irbpair("einstein", "2013-2611"),
            new irbpair("einstein", "2013-2613"),
            new irbpair("einstein", "2013-265"),
            new irbpair("einstein", "2013-2650"),
            new irbpair("einstein", "2013-267"),
            new irbpair("einstein", "2013-2687"),
            new irbpair("einstein", "2013-2701"),
            new irbpair("einstein", "2013-2716"),
            new irbpair("einstein", "2013-2721"),
            new irbpair("einstein", "2013-2724"),
            new irbpair("einstein", "2013-2731"),
            new irbpair("einstein", "2013-2769"),
            new irbpair("einstein", "2013-2773"),
            new irbpair("einstein", "2013-2775"),
            new irbpair("einstein", "2013-2808"),
            new irbpair("einstein", "2013-2819"),
            new irbpair("einstein", "2013-2840"),
            new irbpair("einstein", "2013-2841"),
            new irbpair("einstein", "2013-2869"),
            new irbpair("einstein", "2013-2871"),
            new irbpair("einstein", "2013-2876"),
            new irbpair("einstein", "2013-2916"),
            new irbpair("einstein", "2013-2954"),
            new irbpair("einstein", "2013-2982"),
            new irbpair("einstein", "2013-2986"),
            new irbpair("einstein", "2013-299"),
            new irbpair("einstein", "2014-3008"),
            new irbpair("einstein", "2014-3022"),
            new irbpair("einstein", "2014-3030"),
            new irbpair("einstein", "2014-3050"),
            new irbpair("einstein", "2014-3051"),
            new irbpair("einstein", "2014-3052"),
            new irbpair("einstein", "2014-3056"),
            new irbpair("einstein", "2014-3063"),
            new irbpair("einstein", "2014-3068"),
            new irbpair("einstein", "2014-3109"),
            new irbpair("einstein", "2014-3139"),
            new irbpair("einstein", "2014-3159"),
            new irbpair("einstein", "2014-3175"),
            new irbpair("einstein", "2014-3192"),
            new irbpair("einstein", "2014-3194"),
            new irbpair("einstein", "2014-3196"),
            new irbpair("einstein", "2014-3197"),
            new irbpair("einstein", "2014-3238"),
            new irbpair("einstein", "2014-3239"),
            new irbpair("einstein", "2014-3261"),
            new irbpair("einstein", "2014-3272"),
            new irbpair("einstein", "2014-3281"),
            new irbpair("einstein", "2014-3282"),
            new irbpair("einstein", "2014-3292"),
            new irbpair("einstein", "2014-3300"),
            new irbpair("einstein", "2014-3306"),
            new irbpair("einstein", "2014-3307"),
            new irbpair("einstein", "2014-3320"),
            new irbpair("einstein", "2014-3348"),
            new irbpair("einstein", "2014-3367"),
            new irbpair("einstein", "2014-3384"),
            new irbpair("einstein", "2014-3387"),
            new irbpair("einstein", "2014-3398"),
            new irbpair("einstein", "2014-3404"),
            new irbpair("einstein", "2014-3412"),
            new irbpair("einstein", "2014-3417"),
            new irbpair("einstein", "2014-3442"),
            new irbpair("einstein", "2014-3459"),
            new irbpair("einstein", "2014-3465"),
            new irbpair("einstein", "2014-3479"),
            new irbpair("einstein", "2014-3509"),
            new irbpair("einstein", "2014-3532"),
            new irbpair("einstein", "2014-3588"),
            new irbpair("einstein", "2014-3597"),
            new irbpair("einstein", "2014-3609"),
            new irbpair("einstein", "2014-3611"),
            new irbpair("einstein", "2014-3612"),
            new irbpair("einstein", "2014-3618"),
            new irbpair("einstein", "2014-3625"),
            new irbpair("einstein", "2014-3680"),
            new irbpair("einstein", "2014-3726"),
            new irbpair("einstein", "2014-3736"),
            new irbpair("einstein", "2014-3755"),
            new irbpair("einstein", "2014-3766"),
            new irbpair("einstein", "2014-3798"),
            new irbpair("einstein", "2014-3808"),
            new irbpair("einstein", "2014-3815"),
            new irbpair("einstein", "2014-3820"),
            new irbpair("einstein", "2014-3880"),
            new irbpair("einstein", "2014-3894"),
            new irbpair("einstein", "2014-3902"),
            new irbpair("einstein", "2014-3906"),
            new irbpair("einstein", "2014-3928"),
            new irbpair("einstein", "2014-3930"),
            new irbpair("einstein", "2014-3938"),
            new irbpair("einstein", "2014-3981"),
            new irbpair("einstein", "2014-4010"),
            new irbpair("einstein", "2014-4020"),
            new irbpair("einstein", "2014-4029"),
            new irbpair("einstein", "2014-4037"),
            new irbpair("einstein", "2014-4065"),
            new irbpair("einstein", "2014-4075"),
            new irbpair("einstein", "2014-4091"),
            new irbpair("einstein", "2014-4097"),
            new irbpair("einstein", "2014-4111"),
            new irbpair("einstein", "2014-4133"),
            new irbpair("einstein", "2014-4138"),
            new irbpair("einstein", "2014-4152"),
            new irbpair("einstein", "2014-4168"),
            new irbpair("einstein", "2014-4236"),
            new irbpair("einstein", "2014-4256"),
            new irbpair("einstein", "2014-4262"),
            new irbpair("einstein", "2014-4271"),
            new irbpair("einstein", "2014-4287"),
            new irbpair("einstein", "2014-4294"),
            new irbpair("einstein", "2014-4318"),
            new irbpair("einstein", "2014-4321"),
            new irbpair("einstein", "2014-4326"),
            new irbpair("einstein", "2014-4331"),
            new irbpair("einstein", "2014-4335"),
            new irbpair("einstein", "2014-4343"),
            new irbpair("einstein", "2014-4357"),
            new irbpair("einstein", "2014-4363"),
            new irbpair("einstein", "2014-4374"),
            new irbpair("einstein", "2014-4392"),
            new irbpair("einstein", "2015-4440"),
            new irbpair("einstein", "2015-4449"),
            new irbpair("einstein", "2015-4450"),
            new irbpair("einstein", "2015-4491"),
            new irbpair("einstein", "2015-4493"),
            new irbpair("einstein", "2015-4528"),
            new irbpair("einstein", "2015-4568"),
            new irbpair("einstein", "2015-4570"),
            new irbpair("einstein", "2015-4583"),
            new irbpair("einstein", "2015-4687"),
            new irbpair("einstein", "2015-4691"),
            new irbpair("einstein", "2015-4714"),
            new irbpair("einstein", "2015-4718"),
            new irbpair("einstein", "2015-4728"),
            new irbpair("einstein", "2015-4737"),
            new irbpair("einstein", "2015-4770"),
            new irbpair("einstein", "2015-4774"),
            new irbpair("einstein", "2015-4778"),
            new irbpair("einstein", "2015-4829"),
            new irbpair("einstein", "2015-4836"),
            new irbpair("einstein", "2015-4868"),
            new irbpair("einstein", "2015-4899"),
            new irbpair("einstein", "2015-4902"),
            new irbpair("einstein", "2015-4926"),
            new irbpair("einstein", "2015-4927"),
            new irbpair("einstein", "2015-4929"),
            new irbpair("einstein", "2015-4946"),
            new irbpair("einstein", "2015-4947"),
            new irbpair("einstein", "2015-4949"),
            new irbpair("einstein", "2015-4962"),
            new irbpair("einstein", "2015-4988"),
            new irbpair("einstein", "2015-4989"),
            new irbpair("einstein", "2015-5001"),
            new irbpair("einstein", "2015-5015"),
            new irbpair("einstein", "2015-5019"),
            new irbpair("einstein", "2015-5025"),
            new irbpair("einstein", "2015-5032"),
            new irbpair("einstein", "2015-5035"),
            new irbpair("einstein", "2015-5062"),
            new irbpair("einstein", "2015-5090"),
            new irbpair("einstein", "2015-5170"),
            new irbpair("einstein", "2015-5182"),
            new irbpair("einstein", "2015-5218"),
            new irbpair("einstein", "2015-5238"),
            new irbpair("einstein", "2015-5286"),
            new irbpair("einstein", "2015-5296"),
            new irbpair("einstein", "88-42"),
            new irbpair("einstein", "89-138"),
            new irbpair("einstein", "94-103"),
            new irbpair("einstein", "99-241"),
            new irbpair("einstein", "99-97"),
            new irbpair("einstein", "2014-3185")
        };

        public static readonly bool checkConsentAgentAndDevice = true;

        public static readonly List<string> ignoredIrbNumbers = new List<string>()
        {
            "00-01-018",
            "00-02-058",
            "00-03-087",
            "00-03-088",
            "00-04-111",
            "00-04-120",
            "00-05-122",
            "00-05-127",
            "00-05-129",
            "00-05-130",
            "00-08-276",
            "00-09-291",
            "00-09-294",
            "00-09-297",
            "00-10-318",
            "00-10-330",
            "00-11-350",
            "00-11-351",
            "00-11-358",
            "00-12-379",
            "00-12-380",
            "00-12-392",
            "01-01-403",
            "01-01-413",
            "01-01-414",
            "01-01-430",
            "01-02-038",
            "01-03-062",
            "01-03-063",
            "01-03-068",
            "01-04-073",
            "01-05-101",
            "01-05-102",
            "01-06-123",
            "01-06-140",
            "01-07-171",
            "01-07-174",
            "01-08-175",
            "01-08-182",
            "01-08-207",
            "01-09-220",
            "01-09-223",
            "01-10-240",
            "01-10-251",
            "01-11-259",
            "01-11-265",
            "01-11-293",
            "01-12-304",
            "02-01-010",
            "02-02-052",
            "02-02-053",
            "02-03-068",
            "02-03-079",
            "02-04-098",
            "02-04-105",
            "02-09-249",
            "02-12-312",
            "02-20-010",
            "02-29-010",
            "02-75-010",
            "03-02-033",
            "03-02-034",
            "03-03-060",
            "03-03-065",
            "03-03-066",
            "03-03-067",
            "03-04-090",
            "03-06-132",
            "03-06-133",
            "03-06-141",
            "03-07-160",
            "03-08-182",
            "03-08-203",
            "03-09-230",
            "03-10-250",
            "03-10-251",
            "03-10-255",
            "03-10-256",
            "03-10-258",
            "03-10-277",
            "03-11-289",
            "03-11-290",
            "03-11-291",
            "04-02-024",
            "04-02-027",
            "04-02-028",
            "04-04-083",
            "04-04-084",
            "04-04-085",
            "04-04-091",
            "04-04-106",
            "04-05-113",
            "04-05-116",
            "04-05-125",
            "04-05-129",
            "04-05-131",
            "04-06-160",
            "04-07-186",
            "04-08-227",
            "04-09-267",
            "04-13-002",
            "05-02-059",
            "05-02-061",
            "05-02-062",
            "05-02-191",
            "05-03-077",
            "05-03-082",
            "05-10-151",
            "05-10-277",
            "05-11-297",
            "05-12-330",
            "05-12-332",
            "05-12-347",
            "05-12-348",
            "06-01-010",
            "06-01-012",
            "06-02-480",
            "06-03-010",
            "06-03-072",
            "06-04-243",
            "06-04-258",
            "06-05-280",
            "06-05-291",
            "06-05-295",
            "06-05-296",
            "06-06-129",
            "06-07-342",
            "06-08-373",
            "06-11-468",
            "06-11-476",
            "06-12-491",
            "06-19-010",
            "06-20-010",
            "06-20-701",
            "06-67-010",
            "07-03-080",
            "07-04-101",
            "07-04-115",
            "07-06-160",
            "07-06-164",
            "07-06-169",
            "07-06-251",
            "07-06-257",
            "07-07-206",
            "07-10-368",
            "07-11-437",
            "08-03-053",
            "08-03-076",
            "08-04-097",
            "08-08-281",
            "08-12-429",
            "09-07-199",
            "09-10-306",
            "09-12-392",
            "10-02-022",
            "10-08-262",
            "11-01-001",
            "11-01-021",
            "11-05-182",
            "11-06-111",
            "11-06-159",
            "11-06-229",
            "11-06-233",
            "11-06-242",
            "11-06-262",
            "11-06-287",
            "11-06-318",
            "11-06-950",
            "11-11-218",
            "12-02-040",
            "12-02-285",
            "12-03-071",
            "12-06-130",
            "12-06-179",
            "12-06-193",
            "12-06-218",
            "12-06-253",
            "12-06-300",
            "12-06-303",
            "12-06-320",
            "12-06-324",
            "12-06-341",
            "12-06-366",
            "12-06-373",
            "12-06-399",
            "12-06-600",
            "12-10-230",
            "13-01-047",
            "13-06-110",
            "13-06-142",
            "13-06-145",
            "13-06-193",
            "13-06-297",
            "13-06-328",
            "13-06-790",
            "13-08-147",
            "13-08-327",
            "14-06-320",
            "14-06-340",
            "20-07-405",
            "20-07-418",
            "20-07-554",
            "20-07-559",
            "20-07-579",
            "20-07-587",
            "20-07-607",
            "20-08-359",
            "20-08-411",
            "20-08-423",
            "20-08-492",
            "20-08-493",
            "20-08-525",
            "20-08-526",
            "20-08-534",
            "20-08-565",
            "20-09-236",
            "20-09-265",
            "20-09-272",
            "20-09-279",
            "20-09-455",
            "20-09-485",
            "20-09-500",
            "20-09-541",
            "20-10-276",
            "20-10-326",
            "20-10-349",
            "20-10-379",
            "20-10-400",
            "20-10-446",
            "20-10-567",
            "20-11-355",
            "20-11-387",
            "20-11-404",
            "20-11-552",
            "20-11-559",
            "20-11-629",
            "20-12-278",
            "20-12-436",
            "20-12-471",
            "20-12-547",
            "20-12-630",
            "20-12-680",
            "20-13-248",
            "20-13-299",
            "20-13-314",
            "20-13-381",
            "71-32-089",
            "71-43-023",
            "81-08-105",
            "82-03-034",
            "83-10-185",
            "85-08-170",
            "85-08-175",
            "85-09-199",
            "85-09-201",
            "85-11-240",
            "85-11-243",
            "85-11-246",
            "86-03-076",
            "86-03-091",
            "86-06-151",
            "86-08-231",
            "86-09-238",
            "86-12-333",
            "87-01-008",
            "87-06-154",
            "87-06-157",
            "87-07-185",
            "87-08-193",
            "87-09-231",
            "87-09-239",
            "87-10-259",
            "87-12-337",
            "89-02-035",
            "89-04-101",
            "89-05-123",
            "89-08-187",
            "89-08-198",
            "89-10-256",
            "90-02-057",
            "90-06-164",
            "91-01-008",
            "91-02-031",
            "91-05-156",
            "91-10-278",
            "92-01-015",
            "92-01-016",
            "92-04-098",
            "92-04-099",
            "92-04-128",
            "92-09-288",
            "93-01-006",
            "93-04-104",
            "93-05-131",
            "93-07-183",
            "93-08-223",
            "93-11-322",
            "93-12-457",
            "94-01-015",
            "94-01-037",
            "94-06-219",
            "94-06-220",
            "94-06-237",
            "94-06-246",
            "94-07-264",
            "94-07-279",
            "94-09-352",
            "94-11-412",
            "94-11-433",
            "94-11-434",
            "94-12-488",
            "95-01-003",
            "95-02-044",
            "95-02-045",
            "95-04-139",
            "95-05-175",
            "95-05-178",
            "95-06-223",
            "95-07-248",
            "95-07-262",
            "95-09-340",
            "95-09-360",
            "95-10-396",
            "95-11-422",
            "95-11-433",
            "95-11-455",
            "95-12-481",
            "96-01-003",
            "96-01-020",
            "96-03-090",
            "96-04-118",
            "96-04-120",
            "96-06-191",
            "96-06-202",
            "96-06-204",
            "96-07-232",
            "96-07-240",
            "96-08-258",
            "96-09-311",
            "96-09-313",
            "97-02-039",
            "97-02-040",
            "97-03-074",
            "97-04-137",
            "97-04-138",
            "97-04-139",
            "97-04-149",
            "97-05-174",
            "97-05-176",
            "97-05-198",
            "97-06-206",
            "97-06-213",
            "97-06-216",
            "97-06-222",
            "97-06-223",
            "97-06-241",
            "97-06-242",
            "97-07-254",
            "97-07-264",
            "97-07-265",
            "97-07-266",
            "97-07-267",
            "97-09-363",
            "97-09-382",
            "97-12-468",
            "97-12-470",
            "98-01-002",
            "98-02-018",
            "98-03-076",
            "98-04-077",
            "98-04-088",
            "98-05-107",
            "98-05-110",
            "98-05-111",
            "98-05-129",
            "98-06-138",
            "98-06-143",
            "98-06-145",
            "98-06-147",
            "98-06-148",
            "98-07-181",
            "98-07-186",
            "98-08-213",
            "98-08-214",
            "98-08-216",
            "98-08-240",
            "98-09-245",
            "98-10-291",
            "98-10-292",
            "98-11-317",
            "98-11-322",
            "98-11-325",
            "98-11-327",
            "98-12-344",
            "98-12-351",
            "99-01-001",
            "99-01-013",
            "99-01-015",
            "99-01-016",
            "99-02-820",
            "99-03-067",
            "99-03-069",
            "99-03-070",
            "99-04-132",
            "99-06-179",
            "99-06-186",
            "99-06-190",
            "99-06-195",
            "99-06-213",
            "99-07-249",
            "99-07-255",
            "99-08-262",
            "99-08-269",
            "99-08-270",
            "99-08-286",
            "99-09-302",
            "99-09-304",
            "99-09-335",
            "99-10-349",
            "99-10-350",
            "99-11-376",
            "99-11-383",
            "99-11-395",
            "99-11-404"
        };

        public static readonly List<irbpair> ignoredStudys = new List<irbpair>()
        {
           
        };

        public static readonly List<irbpair> studyToInclude = new List<irbpair>()
        {
            
          
        };

        public static readonly List<string> cancerTerms = new List<string>()
        {
            "oncol",
            "cancer",
            "tumor",
            "carci",
            "leukem",
            "lymphom",
            "myeloma",
            "sarcom",
            "melanom",
            "metast",
            "chemoth",
            "radioth",
            "neuroblast",
            "glioma",
            "carcin",
            "blastom",
            "malignan",
            "myelofibrosis"
        };


        public static readonly List<string> ignoredStatus = new List<string>()
        {
            "Draft",
            "Withdrawn",
            "Administratively Complete",
            "Complete",
            "Archived",
            "Exempt"
        };



        public static readonly Dictionary<string, RCSCPI> migrationStudysRCSCPI = new Dictionary<string, RCSCPI>()
        {
           {"", new RCSCPI("","","")},
{"42f28a55-7f67-4eb3-b2f3-597e1ebc750c", new RCSCPI("Cristina Garcia-Miller","","")},
{"e14e8bac-c232-4520-985c-4912448c1293", new RCSCPI("Cristina Garcia-Miller","","")},
{"2b95af21-596f-4a85-8683-186c7dc22984", new RCSCPI("Cristina Garcia-Miller","","")},

{"ad1ecf6d-ad04-4ac3-a490-24fe60417b7b", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"b48a71c5-9472-43e8-8acd-c3856ced05f1", new RCSCPI("Cristina Garcia-Miller","","")},
{"f87c22ed-a98f-428b-b037-5ee85680b9ac", new RCSCPI("Eileen Burke","","Mark Einstein")},
{"6712db4f-91f8-4f9a-93ed-ab5264e6229c", new RCSCPI("Cristina Garcia-Miller","","")},
{"29263093-7558-4101-8e0f-ae99b55ef2e2", new RCSCPI("","","Ira Braunschweig")},

{"492b2139-1331-4f96-8e44-0f15e2b0320b", new RCSCPI("","","Ira Braunschweig")},
{"68eb2982-af9a-4a60-9134-f6d42e0e1774", new RCSCPI("Cristina Garcia-Miller","","Jesus Anampa")},
{"da1a6bfc-8572-4e60-b213-79470b5b5c8f", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"d23918db-f657-4d49-90a3-3afb1795a18c", new RCSCPI("Cristina Garcia-Miller","","")},
{"8a9047ac-d3a1-4e55-87da-695c91390616", new RCSCPI("Cristina Garcia-Miller","","")},
{"2387e270-bd8b-4fca-8cb0-3a00292c7249", new RCSCPI("","","Murali Janakiram")},
{"d40f7559-2346-4133-81cb-52abd9d92854", new RCSCPI("","","Murali Janakiram")},
{"469c3f44-1aba-4055-aee2-7b0dd3df1c0e", new RCSCPI("","","Heather Ann Smith")},
{"4189cd01-0b47-4ea2-9fa3-d515403459d0", new RCSCPI("Eileen Burke","","")},
{"e987f815-ecd1-4cef-afcb-bc01c522eb7b", new RCSCPI("Cristina Garcia-Miller","","")},

{"d4423ac9-3c75-4314-a4d8-787390ed7509", new RCSCPI("Cristina Garcia-Miller","","")},

{"9e106fe6-eaa6-46f0-8456-63f5ee0e2137", new RCSCPI("Cristina Garcia-Miller","","")},
{"584bb7c1-b81a-4570-9916-f9e4dd05865c", new RCSCPI("Cristina Garcia-Miller","","")},
{"33114bed-54e1-4322-8da6-895add0e589e", new RCSCPI("Cristina Garcia-Miller","","")},
{"29ddff54-80ee-4760-8e8d-245ac3b069bf", new RCSCPI("Cristina Garcia-Miller","","")},

{"cb854309-8ecd-490f-93d6-933685fd3739", new RCSCPI("Cristina Garcia-Miller","","")},
{"05f5a9e4-c720-4b1d-a8e5-c76fd5bd3829", new RCSCPI("Daniel Paucar","","")},

{"5f7950cd-13ba-459c-9d02-4f47e9d5c63f", new RCSCPI("Cristina Garcia-Miller","","")},
{"56228c59-b3d4-4adb-ae69-5cefbb035724", new RCSCPI("Cristina Garcia-Miller","","")},

{"0129e82e-6439-451a-9889-41b773defa24", new RCSCPI("Alexandra Urman","","")},
{"5054a963-3644-4302-880f-d7c70420d810", new RCSCPI("Cristina Garcia-Miller","","")},
{"8ab5711a-184e-4d4b-937c-c02f1d867190", new RCSCPI("Cristina Garcia-Miller","","")},
{"b9362a77-cc5e-462e-bcee-d2cf655e2198", new RCSCPI("Cristina Garcia-Miller","","")},
{"5fbafe54-fb38-4b81-8b6d-bf1ad5a85cca", new RCSCPI("Cristina Garcia-Miller","","")},
{"eb4b9b34-3f54-4675-880a-b43a1725aee7", new RCSCPI("Cristina Garcia-Miller","","")},

{"9bd3cd8a-d97e-4d59-b9a8-6d7ea4bcceab", new RCSCPI("Cristina Garcia-Miller","","Jesus Anampa")},
{"def46869-b642-4e55-94c9-44c6dc6a9a00", new RCSCPI("Cristina Garcia-Miller","","")},

{"7d32649f-8eed-4a5d-97fb-4bc43af8a929", new RCSCPI("Randall Teeter","","Nicole Nevadunsky")},
{"47139787-92ed-46b9-8f39-a0cc5e2b3658", new RCSCPI("Cristina Garcia-Miller","","")},
{"cdc21c44-f840-41bb-8a0f-766adca33023", new RCSCPI("Cristina Garcia-Miller","","")},
{"171371f2-2d15-4480-b9e6-40f7b6e0e64c", new RCSCPI("Cristina Garcia-Miller","","")},


{"d909de99-330a-4391-9e03-8f1dccff4aad", new RCSCPI("Cristina Garcia-Miller","","")},
{"1b761de8-918b-4756-86ed-49bb5ebac2ed", new RCSCPI("Cristina Garcia-Miller","","")},
{"f5f39f81-3305-47b6-9ede-77dcf77e8540", new RCSCPI("","","Ira Braunschweig")},
{"02be8429-50a5-497e-80c6-49ba58d10264", new RCSCPI("Cristina Garcia-Miller","","")},
{"39a6d74f-90a1-4f04-81e8-c05b0e06fa57", new RCSCPI("Cristina Garcia-Miller","","Andreas Kaubisch")},
{"1286a741-af55-4a32-8e9a-d85d81c4824a", new RCSCPI("Cristina Garcia-Miller","","")},


{"6adf013f-08a2-4fdc-b24c-696c4e1f63ce", new RCSCPI("Cristina Garcia-Miller","","")},
{"dd0e38f8-2eb4-4b66-8891-d6f4f1b688fa", new RCSCPI("Cristina Garcia-Miller","","")},
{"98bd8373-d609-45ed-adab-eefb32ec2235", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"d48e43ab-36cc-44a1-901b-ffd93fc1cf87", new RCSCPI("Cristina Garcia-Miller","","")},
{"268906d1-be98-4a79-8bb3-9b767408b937", new RCSCPI("Cristina Garcia-Miller","","")},
{"e8e08d6d-b878-497f-9e36-0eebe3dabb86", new RCSCPI("Cristina Garcia-Miller","","")},

{"2bd0d6c6-66dd-4fc0-a9f0-820975a95a8e", new RCSCPI("Cristina Garcia-Miller","","")},
{"d7e6a654-1f40-4195-936a-17ce34d4c562", new RCSCPI("Cristina Garcia-Miller","","")},
{"6fe2a311-342e-4f34-a9ca-3a9a8d1a4fbb", new RCSCPI("Cristina Garcia-Miller","","")},
{"ff9d977d-4293-408f-b10f-fef41526be66", new RCSCPI("Cristina Garcia-Miller","","")},

//{"8bda9c3f-97ef-4844-93a8-bd8503be9f6a", new RCSCPI("Mark Einstein","","Mark Einstein")},
{"8bda9c3f-97ef-4844-93a8-bd8503be9f6a", new RCSCPI("Nicole Nevadunsky","","Nicole Nevadunsky")},

{"167079a7-e146-45cd-be09-4bef594c4ed5", new RCSCPI("Cristina Garcia-Miller","","")},
{"75e7950b-7844-438c-9584-58c2ad100d8f", new RCSCPI("Cristina Garcia-Miller","","")},


{"2cd26b95-7790-49d0-b3e2-1d0f1b2ea363", new RCSCPI("","","Mark Einstein")},


{"9659e5bd-0097-4864-890f-d2504828dc66", new RCSCPI("Randall Teeter","","Heather Ann Smith")},
{"695be63a-286c-42fe-8dcb-168384ca4d75", new RCSCPI("Cristina Garcia-Miller","","")},
{"5a08ef32-9b4f-4d48-a891-b1adebf4e5d9", new RCSCPI("Cristina Garcia-Miller","","")},
{"f4e9c166-174f-4129-9fb8-a57e0a34159e", new RCSCPI("Cristina Garcia-Miller","","")},



{"caf22a0c-fa8d-4fbf-85c3-28c0aba4a3eb", new RCSCPI("","","Murali Janakiram")},
{"871ae38a-7289-4420-82c1-bb45e55ae740", new RCSCPI("Joshua Levitt","","")},
{"27520241-9eb8-410f-be95-c93a26b36706", new RCSCPI("Cristina Garcia-Miller","","")},
{"56693f1e-1330-4a78-95f9-596effc56dd3", new RCSCPI("Cristina Garcia-Miller","","")},
{"f194dd75-a95c-4e14-8c4a-8ccf6ce48f62", new RCSCPI("Cristina Garcia-Miller","","")},
{"6a53a02e-0acf-481b-ae22-f351a91e2809", new RCSCPI("","","Ira Braunschweig")},
{"3479", new RCSCPI("Peter Cole","","Peter Cole")},
{"3508", new RCSCPI("Peter Cole","","Peter Cole")},
{"3557", new RCSCPI("Peter Cole","","Peter Cole")},



{"3635", new RCSCPI("Shakira Forde","","")},
{"3664", new RCSCPI("Peter Cole","","Peter Cole")},
{"3680", new RCSCPI("","","Murali Janakiram")},


{"3279", new RCSCPI("","","Joseph Sparano")},
{"3696", new RCSCPI("","","Merieme Klobocista")},


{"3713", new RCSCPI("Shakira Forde","","")},
{"3733", new RCSCPI("Shakira Forde","","")},


{"3750", new RCSCPI("Steven Libutti","","")},
{"3758", new RCSCPI("Samantha Feliz","","")},
{"4161", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"3767", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"3060", new RCSCPI("Shakira Forde","","")},


{"3061", new RCSCPI("Shakira Forde","","")},

{"3170", new RCSCPI("Steven Libutti","","")},



{"4153", new RCSCPI("Randall Teeter","","Mark Einstein")},
{"3823", new RCSCPI("Samantha Feliz","","")},


{"3831", new RCSCPI("Peter Cole","","Peter Cole")},
{"3832", new RCSCPI("Peter Cole","","Peter Cole")},
{"3844", new RCSCPI("Shakira Forde","","")},
{"3855", new RCSCPI("Samantha Feliz","","Peter Cole")},
{"3856", new RCSCPI("Peter Cole","","Peter Cole")},
{"3870", new RCSCPI("Peter Cole","","Peter Cole")},
{"3902", new RCSCPI("Joyce Brown","","")},


{"2963", new RCSCPI("Samantha Feliz","","")},
{"3430", new RCSCPI("Samantha Feliz","","")},


{"2956", new RCSCPI("Shakira Forde","","")},
{"3945", new RCSCPI("","","Murali Janakiram")},
{"3950", new RCSCPI("","","Merieme Klobocista")},
{"3954", new RCSCPI("Peter Cole","","Peter Cole")},

{"3959", new RCSCPI("Michelle Goggin","","")},

{"3970", new RCSCPI("Elaine Keung","","")},
{"3971", new RCSCPI("Shakira Forde","","")},

{"3978", new RCSCPI("Samantha Feliz","","")},
{"3984", new RCSCPI("Shakira Forde","","Ira Braunschweig")},
{"3989", new RCSCPI("Cristina Garcia-Miller","","")},
{"3993", new RCSCPI("Stuart Packer","","")},


{"1166", new RCSCPI("Samantha Feliz","","")},

{"3013", new RCSCPI("Randall Teeter","","Mark Einstein")},





{"3172", new RCSCPI("Steven Libutti","","")},
{"3173", new RCSCPI("Steven Libutti","","")},

{"3162", new RCSCPI("Shakira Forde","","")},
{"3287", new RCSCPI("Shakira Forde","","")},
{"1312", new RCSCPI("Shakira Forde","","")},


{"1507", new RCSCPI("Michelle Goggin","","")},


{"2208", new RCSCPI("Shakira Forde","","")},
{"2209", new RCSCPI("Shakira Forde","","")},
{"2246", new RCSCPI("Samantha Feliz","","")},
{"2363", new RCSCPI("Samantha Feliz","","")},
{"2445", new RCSCPI("Joyce Brown","","")},
{"2805", new RCSCPI("Joyce Brown","","")},
{"2820", new RCSCPI("Samantha Armstrong","","")},
{"4139", new RCSCPI("Shakira Forde","","")},
{"4322", new RCSCPI("Michelle Goggin","","")},

{"4419", new RCSCPI("Samantha Feliz","","Mark Einstein")},

{"4619", new RCSCPI("Alexandra Urman","","Ira Braunschweig")},

{"4632", new RCSCPI("Samantha Feliz","","")},

{"1948", new RCSCPI("Samantha Feliz","","Mark Einstein")},

{"7f0f8edc-43fe-4bf5-9f72-ed9b3c17b413", new RCSCPI("Ambar Baez","","Olga Derman")},











{"284cad69-e0c0-4c01-99ab-9a7faab371dd", new RCSCPI("","","Olga Derman")},



{"ff324b9f-3cd8-49cd-81af-15e9b79466ec", new RCSCPI("","","Olga Derman")},
















{"7067f3a2-523c-45c0-b64a-c6fde4d0adee", new RCSCPI("","","Jerome Graber")},
{"d34582e3-35c2-421c-87ca-c9539dd79fce", new RCSCPI("","","Alexander I. Sankin")},










{"48b4b763-4785-4256-b83f-c0d31ecff84b", new RCSCPI("Elaine Keung","","")},








































{"3087", new RCSCPI("","","Jerome Graber")},













{"3978", new RCSCPI("","","Olga Derman")},










{"3261", new RCSCPI("","","Nitin Ohri")},






{"3287", new RCSCPI("","","Roman Perez-Soler")},












{"4139", new RCSCPI("","","Roman Perez-Soler")},


{"4432", new RCSCPI("","","Jerome Graber")},


{"4632", new RCSCPI("","","Nitin Ohri")}



        };

        public static RCSCPI getRCSCPI(string studyid)
        {
            RCSCPI rcscpi = new RCSCPI();
            if (SpecialStudys.migrationStudysRCSCPI.ContainsKey(studyid))
            {
                rcscpi = SpecialStudys.migrationStudysRCSCPI[studyid];
            }
            return rcscpi;
        }

    }



    public class irbpair
    {
        public string IRB { get; set; }
        public string number { get; set; }

        public irbpair(string irb, string Number)
        {
            IRB = irb;
            number = Number;
        }
    }

    public class RCSCPI
    {
        public string RC { get; set; }
        public string SC { get; set; }
        public string PI { get; set; }

        public RCSCPI(string rc, string sc, string pi)
        {
            RC = rc;
            PI = pi;
            SC = sc;
        }

        public RCSCPI()
        {
            RC = "";
            PI = "";
            SC = "";
        }
    }


}
