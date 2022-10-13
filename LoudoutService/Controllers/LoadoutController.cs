using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoudoutService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadoutController : ControllerBase
    {
        public class LoadoutController : ControllerBase
        {
            [HttpPost("ConvertLoadout")]
            public LoadoutResponse ConvertLoadout(LoadoutRequest request)
            {
                var response = new LoadoutResponse();

                response.ResultText = CustomLoadoutGenerator(request);

                return response;
            }

            private string CustomLoadoutGenerator(LoadoutRequest request)
            {
                string sResult = "";
                string ResultText = "";
                foreach (string strLine in request.TextInput.Split(Environment.NewLine))
                {
                    int iInnerLoopCount = 0;
                    string adjustedString = strLine.Replace(Environment.NewLine, "").Replace("\n", "");

                    if (adjustedString.Substring(0, 7) != "comment" && adjustedString != "" && adjustedString.Substring(0, 6) != "remove")
                    {
                        string sTempResult = adjustedString;
                        if (adjustedString.Substring(0, 3) == "for")
                        {
                            string sLoopCount = adjustedString.Substring(adjustedString.IndexOf("from 1 to ") + 10);
                            sLoopCount = sLoopCount.Substring(sLoopCount.IndexOf(" do")).Trim();
                            if (sLoopCount.All(char.IsNumber))
                                iInnerLoopCount = Convert.ToInt32(sLoopCount) - 1;
                            sTempResult = adjustedString.Substring(adjustedString.IndexOf("{") + 1, adjustedString.Length);
                            sTempResult = sTempResult.Substring(0, sTempResult.IndexOf("}") - 1);
                        }
                        sTempResult = sTempResult.Replace("this ", "");
                        sTempResult = sTempResult.Replace(" \"", "=");
                        sTempResult = sTempResult.Replace("\";", ";");

                        for (int i = 0; i <= iInnerLoopCount; i++)
                            sResult += sTempResult + Environment.NewLine;
                    }
                }
                request.TextInput = "";
                return ResultText = "#" + Environment.NewLine + request.UnitType + ":" + Environment.NewLine + sResult;
            }
        }

    }
}
