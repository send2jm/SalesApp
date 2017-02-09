using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SalesApp
{

    public class Root
    {
        [XmlElement("Study")]
        public List<Study> Study { get; set; }
    }

    public class Study
    {
        public string mOID { get; private set; }
        [XmlAttribute("StudyName")]
        public string mStudyName { get; private set; }
        [XmlAttribute("ProtocolName")]
        public int mProtocolName { get; private set; }

        public Study(string OID)
        {
            mOID = OID;

        }

        public Study(string OID, string StudyName, int ProtocolName, bool Study)
        {
            mOID = OID;
            mStudyName = StudyName;
            mProtocolName = ProtocolName;
            
        }
    }


    public class XmlSerializerHelper<T> where T : class
    {
        private readonly XmlSerializer _serializer;

        public XmlSerializerHelper()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public T BytesToObject(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var reader = new XmlTextReader(memoryStream))
                {
                    return (T)_serializer.Deserialize(reader);
                }
            }
        }
    }
    
    
}
