﻿using System;
using Microsoft.AspNetCore.Http;

namespace SettlementBookingSystem.ProblemDetails
{
    public class UnhandledExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public UnhandledExceptionProblemDetails(Exception ex)
        {
            Status = StatusCodes.Status500InternalServerError;
            Title = "Internal Server Error";
            Detail = ex.Message;
            Type = "https://httpstatuses.com/500";
        }
    }
}
