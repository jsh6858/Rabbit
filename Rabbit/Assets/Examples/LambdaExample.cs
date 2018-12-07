//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

using UnityEngine;
using UnityEngine.UI;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using Amazon.Lambda.Model;

namespace AWSSDK.Examples
{
    public class LambdaExample : MonoBehaviour
    {
        public string IdentityPoolId = "";
        public string CognitoIdentityRegion = RegionEndpoint.APNortheast1.SystemName;
        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }
        public string LambdaRegion = RegionEndpoint.APNortheast1.SystemName;
        private RegionEndpoint _LambdaRegion
        {
            get { return RegionEndpoint.GetBySystemName(LambdaRegion); }
        }


        void Start()
        {
            

            UnityInitializer.AttachToGameObject(this.gameObject);

            Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;

            Invoke("TestFunc", "{\"key1\":\"kwon\"}");
        }

        private IAmazonLambda _lambdaClient;
        private AWSCredentials _credentials;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
                return _credentials;
            }
        }

        public Text m_result;

        private IAmazonLambda Client
        {
            get
            {
                if (_lambdaClient == null)
                {
                    _lambdaClient = new AmazonLambdaClient(Credentials, _LambdaRegion);
                }
                return _lambdaClient;
            }
        }

        public void Invoke(string func, string eventText)
        {
            Client.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = func,
                Payload = eventText
            },
            (response) =>
            {
                if (response.Exception == null)
                {
                    string result = Encoding.ASCII.GetString(response.Response.Payload.ToArray());

                    m_result.text = result;
                    Debug.Log(result);
                }
                else
                {
                    Debug.Log(response.Exception);
                }
            });
        }
    }
}
