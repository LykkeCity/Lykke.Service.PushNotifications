using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PushNotifications.Contract.Enums
{
    public enum NotificationType
    {
        Info = 0,
        KycSucceess,
        KycRestrictedArea,
        KycNeedToFillDocuments,
        TransctionFailed,
        TransactionConfirmed,
        AssetsCredited,
        BackupWarning,
        EthNeedTransactionSign,
        PositionOpened,
        PositionClosed,
        MarginCall,
        OffchainRequest,
        NeedTransactionSign,
        PushTxDialog,
        LimitOrderEvent,
        LiveAvailable,
        ClientDialog,
        OperationCreated,
        TradingSessionCreated
    }
}
