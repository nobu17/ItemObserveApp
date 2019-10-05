using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class AWSLoginRepository : ILoginRepository
    {
        public AWSLoginRepository()
        {
        }

        public async Task<LoginResult> LoginAsync(string userID, string password)
        {
            var result = new LoginResult();
            try
            {
                var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.APNortheast1);
                CognitoUserPool userPool = new CognitoUserPool(Util.Enviroment.CognitoUserPoolId, Util.Enviroment.CognitoClientId, provider);
                CognitoUser user = new CognitoUser(userID, Util.Enviroment.CognitoClientId, userPool, provider);
                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
                {
                    Password = password
                };

                var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
                var accessToken = "";
                if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    authResponse = await user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
                    {
                        SessionID = authResponse.SessionID,
                        NewPassword = password
                    });
                    accessToken = authResponse.AuthenticationResult.AccessToken;
                }
                else if (authResponse.AuthenticationResult.AccessToken != null)
                {
                    accessToken = authResponse.AuthenticationResult.AccessToken;
                }
                else
                {
                    result.IsSuccessed = false;
                    result.Message = "UnExcepted error";
                    return result;
                }

                result.IsSuccessed = true;
                result.Message = string.Empty;
                result.Token = accessToken;
                return result;
            }
            catch (NotAuthorizedException)
            {
                result.IsSuccessed = false;
                result.Message = "userid or password is incorrect";
                return result;
            }
            catch (Exception e)
            {
                result.IsSuccessed = false;
                result.Message = e.Message;
                return result;
            }
        }
    }
}
