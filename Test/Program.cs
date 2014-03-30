using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Test
{
    [Serializable]
    [XmlRoot(ElementName = "n")]
    public class Node
    {
        [XmlAttribute(AttributeName = "v")]
        public string Name { get; set; }

        [XmlArray("c")]
        [XmlArrayItem(ElementName="n")]
        public Node[] Children { get; set; }

        public Node(string name)
        {
            Name = name;
            Children = new Node[2];
        }

        public Node()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node a = new Node("a");
            Node b = new Node("b");
            Node c = new Node("c");
            Node d = new Node("d");
            a.Children[0] = b;
            a.Children[1] = d;
            b.Children[1] = c;

            XmlSerializer xs = new XmlSerializer(typeof(Node));

            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                xs.Serialize(tw, a);
            }
            
            string xml = sb.ToString();

            using (TextReader sr = new StringReader(xml))
            {
                Node s = (Node)xs.Deserialize(sr);
            }

            Console.WriteLine(xml);
            Console.ReadLine();
        }
    }
}
