namespace King.Service
{
    using System;

    /// <summary>
    /// Remarks:
    /// Must be placed on class with a default constructor
    /// Must be placed on a method with no parameters
    /// Must be placed on method that returns bool (work was done)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InitializeAttribute : Attribute
    {
    }
}