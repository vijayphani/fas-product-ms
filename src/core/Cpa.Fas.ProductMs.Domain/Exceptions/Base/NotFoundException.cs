﻿namespace Cpa.Fas.ProductMs.Domain.Exceptions.Base;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message)
        : base(message)
    {
    }
}