using System;
using System.Linq;
using System.Threading.Tasks;
using com.amazon.device.iap.cpt;

namespace PlayGround.UI.Droid.Services
{
    public class AmazonIAPService
    {
        public async Task<bool> GetProductInfoAsync(params string[] productIds)
        {
            var iapService = AmazonIapV2Impl.Instance;
            var tcs = new TaskCompletionSource<bool>();
            var skus = new SkusInput { Skus = productIds.ToList() };
            var requestId = iapService.GetProductData(skus).RequestId;

            GetProductDataResponseDelegator delegator = null;
            delegator = new GetProductDataResponseDelegator(response => 
            {
                if(response.RequestId == requestId) {
                    var result = GetResultFromResponse(response);
                    tcs.SetResult(result);
                    //iapService.RemoveGetProductDataResponseListener(delegator.responseDelegate);
                }
            });

            iapService.AddGetProductDataResponseListener(delegator.responseDelegate);
            return await tcs.Task;
        }

        private bool GetResultFromResponse(GetProductDataResponse response)
        {
            // do something useful
            return true;
        }

        /*
         * Using await Task.Delay(TimeSpan.FromMilliseconds(1)); like below
         * seems to solve the problem but it's way to hacky for my taste
         * 
        public async Task<bool> GetProductInfoAsync(params string[] productIds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            var iapService = AmazonIapV2Impl.Instance;
            var tcs = new TaskCompletionSource<bool>();
            var skus = new SkusInput { Skus = productIds.ToList() };
            var requestId = iapService.GetProductData(skus).RequestId;
        
            GetProductDataResponseDelegator delegator = null;
            delegator = new GetProductDataResponseDelegator(async response => 
            {
                if(response.RequestId == requestId) {
                    var result = GetResultFromResponse(response);
                    tcs.SetResult(result);
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                    iapService.RemoveGetProductDataResponseListener(delegator.responseDelegate);
                }
            });
        
            iapService.AddGetProductDataResponseListener(delegator.responseDelegate);
            return await tcs.Task;
        }
        */
    }
}
