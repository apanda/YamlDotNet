using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.EventEmitters;
using System.Diagnostics.Contracts;
public class CommandC
{
}

public class SoftNIC : CommandC
{
    public SNArgs Argument { set; get; }
}
public class NF : CommandC
{
    public NFArgs Argument { set; get; }
}
public class SNArgs
{
    public List<String> Cores { set; get; }
}
public class NFArgs
{
    public int Nfid { get; set; }
    public string Name { get; set; }
    public List<string> Arguments { get; set; }
    public List<int> Cores { get; set; }
}
public class SN
{
    public List<CommandC> Requests { get; set; }
}

namespace YamlDotNet.Samples
{


    public class DeserializeObjectGraph
    {
        public static void Main()
        {
            var input = new StringReader(Doc2);

            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            deserializer.RegisterTagMapping("tag:yaml.org,2002:softnic", typeof(SoftNIC));
            deserializer.RegisterTagMapping("tag:yaml.org,2002:nf", typeof(NF));
            var order = deserializer.Deserialize<SN>(input);
            TypeAssigningEventEmitter.AddTypeMapping(typeof(SN), " ");
            TypeAssigningEventEmitter.AddTypeMapping(typeof(SoftNIC), "!!SN");
            TypeAssigningEventEmitter.AddTypeMapping(typeof(NF), "!!NF");
            var serializer = new Serializer(SerializationOptions.Roundtrip);
            var output = new StringWriter();
            serializer.Serialize(output, order);
            Console.WriteLine("Output \n{0}", output.ToString());
            Console.ReadKey();
        }


        private const string Doc2 = @"---
requests:
  - !!softnic
    argument:
      cores: [0]
  - !!nf
    argument: 
      nfid: 1                                          
      name: firewall
      arguments: [-e]
      cores: [1]
...";
   }
}
