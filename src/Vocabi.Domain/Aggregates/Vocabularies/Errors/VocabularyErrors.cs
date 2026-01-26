using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vocabi.Domain.Aggregates.Vocabularies.Errors;
internal class VocabularyErrors
{
}

public static class FollowerErrors
{
    public static readonly Error SameUser = new Error(
        "Followers.SameUser", "Can't follow yourself");

    public static readonly Error NonPublicProfile = new Error(
        "Followers.NonPublicProfile", "Can't follow non-public profiles");

    public static readonly Error AlreadyFollowing = new Error(
        "Followers.AlreadyFollowing", "Already following");
}