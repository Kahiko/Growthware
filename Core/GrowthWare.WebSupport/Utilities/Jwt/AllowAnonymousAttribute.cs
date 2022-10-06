using System;

namespace GrowthWare.WebSupport.Utilities.Jwt;
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }