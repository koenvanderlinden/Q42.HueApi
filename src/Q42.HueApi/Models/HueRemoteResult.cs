using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class HueRemoteResult
  {
    //success will show: "{\"code\":200,\"message\":\"ok\",\"result\":\"ok\"}"
    //failure could be like: "{\"code\":109,\"message\":\"I don\\u0027t know that token.\",\"result\":\"error\"}"
    //or "{\"code\":113,\"message\":\"Invalid JSON.\",\"result\":\"error\"}"

    public int Code { get; set; }
    public string Message { get; set; }
    public string Result { get; set; }

  }

}
