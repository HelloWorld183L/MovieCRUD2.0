﻿namespace MovieCRUD.SharedKernel
{
    public interface INamedEntity : IEntity
    {
        string Name { get; set; }
    }
}
