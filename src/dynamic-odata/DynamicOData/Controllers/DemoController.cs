using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DynamicOData.Controllers
{
    public class DemoController : Controller
    {
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(new DemoData[]
            {
                new DemoData{Key = "1", Value = "Demo1"}
            });
        }
    }

    [DataContract]
    public class DemoData
    {
        [Key]
        [DataMember(Name = "Schlüssel")]
        public string Key { get; set; }

        [DataMember(Name = "Wert")]
        public string Value { get; set; }
    }
}
