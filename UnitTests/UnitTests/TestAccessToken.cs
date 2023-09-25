using Livekit;
using LiveKit_CSharp.Auth;
using NUnit.Framework;

namespace UnitTests;

public class TestAccessToken
{

    [Test]
    public void TestCanGenerateAccessToken()
    {
        var accessToken = new AccessToken("Alg5qfSGXaqd426", "0adb5eebd3de6f3af994f0ba3e1975c0");

        var videoGrant = new VideoGrant()
        {
            RoomJoin = true,
            Room = Random.Shared.Next().ToString(),
            CanPublish = true,
            CanSubscribe = true,
            CanPublishSources = new List<TrackSource>()
            {
                TrackSource.Camera,
                TrackSource.Microphone,
                TrackSource.ScreenShare,
                TrackSource.ScreenShareAudio
            }
        };

        var jwt = accessToken.AddGrant(videoGrant)
            .SetIdentity("userId")
            .SetTTL(TimeSpan.FromHours(2))
            .SetName("anson").ToJwt();
        
        Assert.That(jwt, Is.Not.Empty);
    }
} 