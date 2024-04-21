using System;

namespace GrowthWare.Web.Support.Jwt;
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }