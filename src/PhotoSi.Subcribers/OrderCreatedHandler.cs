using DomainModel.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSi.Subcribers
{
    public class OrderCreatedHandler : INotificationHandler<OrderConfirmedPublicEvent>
    {
        private readonly ILogger _logger;
        private readonly ISendMail _sendMail;

        public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger, 
                                    ISendMail sendMail)
        {
            _logger = logger;
            _sendMail = sendMail;
        }

        public async Task Handle(OrderConfirmedPublicEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("START OrderCreatedHandler");

            _sendMail.SendMailAsync(notification.Order.OrderId);

            _logger.LogDebug("END OrderCreatedHandler");
        }

    }
}
