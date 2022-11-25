using System;

namespace GrowthWare.WebSupport.Jwt;
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }