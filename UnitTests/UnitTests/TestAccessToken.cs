using Livekit;
using NUnit.Framework;
using LiveKit_CSharp.Auth;
using LiveKit_CSharp.Services.Meeting;

namespace UnitTests;

public class TestAccessToken
{
    [Test]
    public void TestCanGenerateAccessToken()
    {
        var accessToken = new AccessToken("Alg5qfSGXaqd426", "0adb5eebd3de6f3af994f0ba3e1975c0");

        // new RoomService.RoomServiceClient(GrpcChannel.ForAddress("localhost:50051"));

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

    [Test]
    public void TestCanGenerateAccessTokenForMeeting()
    {
        var accessToken = new GenerateAccessToken();

        var createMeetingJwt = accessToken
            .CreateMeeting("5201314", "Alg5qfSGXaqd426", "0adb5eebd3de6f3af994f0ba3e1975c0", "1", "greg");

        var joinMeetingJwt = accessToken
            .JoinMeeting("5201314", "Alg5qfSGXaqd426", "0adb5eebd3de6f3af994f0ba3e1975c0", "1", "greg");
        
        var getAllMeetingJwt = accessToken
            .GetAllMeeting("5201314", "Alg5qfSGXaqd426", "0adb5eebd3de6f3af994f0ba3e1975c0", "1", "greg");
        
        Assert.That(joinMeetingJwt, Is.Not.Empty);
        Assert.That(createMeetingJwt, Is.Not.Empty);
        Assert.That(getAllMeetingJwt, Is.Not.Empty);
    }
} 