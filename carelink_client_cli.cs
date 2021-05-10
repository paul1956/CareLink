namespace Namespace {
    
    using carelink_client;
    
    using argparse;
    
    using datetime;
    
    using time;
    
    using json;
    
    using System;
    
    using System.Linq;
    
    public static class Module {
        
        public static object writeJson(object jsonobj, object name) {
            var filename = name + "-" + datetime.datetime.now().strftime("%Y%m%d_%H%M%S") + ".json";
            try {
                var f = open(filename, "w");
                f.write(json.dumps(jsonobj, indent: 3));
                f.close();
            } catch (Exception) {
                Console.WriteLine("Error saving " + filename + ": " + e.ToString());
                return false;
            }
        }
        
        public static object parser = argparse.ArgumentParser();
        
        static Module() {
            parser.add_argument("--username", "-u", type: str, help: "CareLink username", required: true);
            parser.add_argument("--password", "-p", type: str, help: "CareLink password", required: true);
            parser.add_argument("--country", "-c", type: str, help: "CareLink two letter country code", required: true);
            parser.add_argument("--repeat", "-r", type: @int, help: "Repeat request times", required: false);
            parser.add_argument("--wait", "-w", type: @int, help: "Wait minutes between repeated calls", required: false);
            parser.add_argument("--data", "-d", help: "Save recent data", action: "store_true");
            parser.add_argument("--verbose", "-v", help: "Verbose mode", action: "store_true");
            time.sleep(1);
            time.sleep(1);
            time.sleep(wait * 60);
        }
        
        public static object args = parser.parse_args();
        
        public static object username = args.username;
        
        public static object password = args.password;
        
        public static object country = args.country;
        
        public static object repeat = args.repeat == null ? 1 : args.repeat;
        
        public static object wait = args.wait == null ? 5 : args.wait;
        
        public static object data = args.data;
        
        public static object verbose = args.verbose;
        
        public static object client = carelink_client.CareLinkClient(username, password, country);
        
        public static object recentData = client.getRecentData();
    }
}
