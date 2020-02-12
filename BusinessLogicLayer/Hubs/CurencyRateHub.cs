using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Hubs
{
    public class CurencyRateHub : Hub
    {
        public async Task UpdateCurrencyRate (string CurrencyKey, int CurrencyValue)
        {
            await Clients.All.SendAsync("UpdateCurrencyRate", CurrencyKey, CurrencyValue);
        }

    }
}
