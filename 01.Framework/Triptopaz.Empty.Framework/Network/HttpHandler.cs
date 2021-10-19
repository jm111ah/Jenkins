using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Triptopaz.Empty.Framework.Configuration;

namespace Triptopaz.Empty.Framework.Network
{
    public class HttpHandler : HttpClient
    {
        #region Properties
        
        #endregion Properties

        #region Constructor
        /// <summary>생성자(기본)</summary>
        public HttpHandler() : base()
        {
            base.Timeout = TimeSpan.FromSeconds(CONST.defaultNetworkTimeout);
        }
        /// <summary>생성자(timeout 포함)</summary>
        /// <param name="timeout">타임아웃 시간</param>
        public HttpHandler(int timeout)
        {
            base.Timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>생성자(timeout 포함)</summary>
        /// <param name="timeout">타임아웃 시간</param>
        /// /// <param name="baseUrl">기본 URL</param>
        public HttpHandler(int timeout, Uri baseUrl) : base()
        {
            base.Timeout = TimeSpan.FromSeconds(timeout);
            base.BaseAddress = baseUrl;
        }

        /// <summary>
        /// 생성자 핸들러, 타임아웃 포함
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="handler"></param>
        public HttpHandler(int timeout, HttpClientHandler handler) : base(handler)
        {
            base.Timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// 생성자 핸들러, 타임아웃 포함
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="baseUrl"></param>
        /// <param name="handler"></param>
        public HttpHandler(int timeout, Uri baseUrl, HttpClientHandler handler) : base(handler)
        {
            base.Timeout = TimeSpan.FromSeconds(timeout);
            base.BaseAddress = baseUrl;
        }


        #endregion Constructor

        #region Method
        /// <summary>
        /// HTTP 통신에 사용됨 비동기 TASK 결과를 받아 온다.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var taskResult = base.SendAsync(request, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    // 제한 시간 초과 시  로그 남기는 부분
                }

                return taskResult;
            }
            catch (Exception ex)
            {
                // Exception log
                
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Post로 전송 json 입출력
        /// </summary>
        /// <typeparam name="REQ">요청 모델</typeparam>
        /// <typeparam name="RES">응답 모델</typeparam>
        /// <param name="endpoint">주소</param>
        /// <param name="param">요청 모델 값</param>
        /// <returns>응답 모델 값</returns>
        public async Task<RES> PostJsonAysnc<REQ, RES>(string endpoint, REQ param)
        {
            var json = JsonConvert.SerializeObject(param, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await base.PostAsync(endpoint, requestContent);
            //response.EnsureSuccessStatusCode();

            return await ReadJsonResponseAsync<RES>(response);
        }

        public async Task<RES> PostJsonAysnc<RES>(string endpoint, HttpContent param)
        {
            var response = await base.PostAsync(endpoint, param);
            //response.EnsureSuccessStatusCode();

            return await ReadJsonResponseAsync<RES>(response);
        }

        /// <summary>
        /// Get으로 전송 json 출력
        /// </summary>
        /// <typeparam name="RES">응답 모델</typeparam>
        /// <param name="endpoint">주소</param>
        /// <returns></returns>
        public async Task<RES> GetJsonAsync<RES>(string endpoint)
        {
            var response = await base.GetAsync(endpoint);
            //response.EnsureSuccessStatusCode();
            return await ReadJsonResponseAsync<RES>(response);
        }

        protected async Task<RES> ReadJsonResponseAsync<RES>(HttpResponseMessage response)
        {
            RES ret = default(RES);

            var content = await response.Content.ReadAsStringAsync();
            ret = JsonConvert.DeserializeObject<RES>(content);

            return ret;
        }

        #endregion Method
    }
}
}
