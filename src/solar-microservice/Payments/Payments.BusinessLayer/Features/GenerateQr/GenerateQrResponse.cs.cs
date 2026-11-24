using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Features.GenerateQr
{
    public sealed record GenerateQrResponse(
    int OrderId,
    decimal Amount,
    string TransactionId,
    int ExpiresIn,
    string QrCodeImage);
}
