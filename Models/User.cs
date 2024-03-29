﻿namespace TheOmenDen.Shared.Models;
/// <summary>
/// Provides basic information about a particular user
/// </summary>
/// <param name="Id">The user's unique Id</param>
/// <param name="Email">The user's provided email</param>
/// <param name="Name">The user's name</param>
/// <param name="IsAuthenticated">Checking if the user is authenticated where relevant</param>
/// <param name="Key">A unique integer key for the user's provided information</param>
public sealed record User(Guid Id, String Email, String Name, Boolean IsAuthenticated, Int32 Key): IUser
{
    public bool Equals(User? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id) 
               && Email == other.Email 
               && Key == other.Key;
    }

    public override int GetHashCode() => HashCode.Combine(Id, Email, Key);
}