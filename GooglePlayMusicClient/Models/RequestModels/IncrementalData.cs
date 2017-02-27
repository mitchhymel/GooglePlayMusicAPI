using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.RequestModels
{
    [DataContract]
    public class IncrementalData<T>
    {
        [DataMember(Name = "items")]
        public List<T> Items { get; set; }
    }
}
