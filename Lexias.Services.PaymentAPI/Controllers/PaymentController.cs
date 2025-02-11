﻿using Dapr;
using Dapr.Client;
using Lexias.Services.PaymentAPI.Data.Repository;
using Lexias.Services.PaymentAPI.Data.Repository.IRepository;
using Lexias.Services.PaymentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Enum;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentRepository _db;

        public PaymentController(
            DaprClient daprClient,
            ILogger<PaymentController> logger, 
            IPaymentRepository paymentRepository)
        {
            _daprClient = daprClient;
            _logger = logger;
            _db = paymentRepository;
        }





        [Topic(PaymentChannel.Channel, PaymentChannel.Topics.Payment)]
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentEvent processPaymentEvent)
        {
            _logger.LogInformation("Step1.P: Processing payment: {CorrelationId}, Amount: {Amount}, OrderId: {OrderId}",
                processPaymentEvent.CorrelationId,
                processPaymentEvent.Amount,
                processPaymentEvent.OrderId);



            // Save payment details to database
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                OrderId = processPaymentEvent.OrderId,
                Amount = processPaymentEvent.Amount,
                PaymentDate = DateTime.UtcNow,
                Status = PaymentStatus.Completed // Assuming payment succeeds
            };



            try
            {
                await _db.AddPaymentAsync(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while saving payment for OrderId: {OrderId}. Error: {ErrorMessage}",
                    processPaymentEvent.CorrelationId, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing payment");
            }



            //// For this example, assume payment always succeeds /////
            var paymentProcessedResultEvent = new PaymentProcessedResultEvent
            {
                CorrelationId = processPaymentEvent.CorrelationId,
                Amount = processPaymentEvent.Amount,
                State = ResultState.Succeeded
            };


            // Publish the payment result event to the workflow channel
            await _daprClient.PublishEventAsync(
                WorkflowChannel.Channel,
                WorkflowChannel.Topics.PaymentResult,
                paymentProcessedResultEvent);


            _logger.LogInformation("Step2.P: Payment processed: {CorrelationId}, {Amount}, {State}",
                paymentProcessedResultEvent.CorrelationId, 
                paymentProcessedResultEvent.Amount,
                paymentProcessedResultEvent.State);



            return Ok(paymentProcessedResultEvent);
        }









        //Payment is NOT SUCCED ALWAYS
        //[Topic(PaymentChannel.Channel, PaymentChannel.Topics.Payment)]
        //[HttpPost]
        //public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentEvent processPaymentEvent)
        //{
        //    _logger.LogInformation("Processing payment: {CorrelationId}, Amount: {Amount}",
        //        processPaymentEvent.CorrelationId,
        //        processPaymentEvent.Amount);

        //    var paymentStatus = PaymentStatus.Failed;

        //    try
        //    {
        //        // Create a payment intent with Stripe
        //        var options = new PaymentIntentCreateOptions
        //        {
        //            Amount = (long)(processPaymentEvent.Amount * 100), // Amount in smallest currency unit (øre)
        //            Currency = "dkk", // Use Danish Krone as the currency
        //            PaymentMethodTypes = new List<string> { "card" },
        //            Description = $"Payment for Order {processPaymentEvent.CorrelationId}"
        //        };

        //        var service = new PaymentIntentService();
        //        var paymentIntent = await service.CreateAsync(options);

        //        // Save payment details to the database with pending status
        //        var payment = new Payment
        //        {
        //            PaymentId = Guid.NewGuid().ToString(),
        //            OrderId = processPaymentEvent.CorrelationId,
        //            Amount = processPaymentEvent.Amount,
        //            PaymentDate = DateTime.UtcNow,
        //            Status = PaymentStatus.Pending
        //        };

        //        _context.Orders.Add(payment);
        //        await _context.SaveChangesAsync();

        //        // Assume the payment is successful if no exceptions occurred
        //        payment.Status = PaymentStatus.Completed;
        //        paymentStatus = PaymentStatus.Completed;

        //        // Update the payment status in the database
        //        _context.Orders.Update(payment);
        //        await _context.SaveChangesAsync();

        //        // Create a PaymentProcessedResultEvent with success state
        //        var paymentProcessedResultEvent = new PaymentProcessedResultEvent
        //        {
        //            CorrelationId = processPaymentEvent.CorrelationId,
        //            Amount = processPaymentEvent.Amount,
        //            State = ResultState.Succeeded
        //        };

        //        // Publish the payment result event to the workflow channel
        //        await _daprClient.PublishEventAsync(
        //            WorkflowChannel.Channel,
        //            WorkflowChannel.Topics.PaymentResult,
        //            paymentProcessedResultEvent);

        //        _logger.LogInformation("Payment processed successfully: {CorrelationId}, {Amount}, {State}",
        //            paymentProcessedResultEvent.CorrelationId,
        //            paymentProcessedResultEvent.Amount,
        //            paymentProcessedResultEvent.State);

        //        return Ok(paymentProcessedResultEvent);
        //    }
        //    catch (StripeException ex)
        //    {
        //        _logger.LogError("Stripe payment failed: {CorrelationId}, Error: {Error}",
        //            processPaymentEvent.CorrelationId, ex.Message);

        //        // Create a PaymentProcessedResultEvent with failed state
        //        var paymentProcessedResultEvent = new PaymentProcessedResultEvent
        //        {
        //            CorrelationId = processPaymentEvent.CorrelationId,
        //            Amount = processPaymentEvent.Amount,
        //            State = ResultState.Failed
        //        };

        //        // Publish the payment result event to the workflow channel
        //        await _daprClient.PublishEventAsync(
        //            WorkflowChannel.Channel,
        //            WorkflowChannel.Topics.PaymentResult,
        //            paymentProcessedResultEvent);

        //        return BadRequest(paymentProcessedResultEvent);
        //    }
        //}


    }
}
