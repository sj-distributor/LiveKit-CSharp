using System;
using Livekit;
using LiveKit_CSharp.Auth;
using System.Collections.Generic;

namespace LiveKit_CSharp.Services.Meeting
{
    public class GenerateAccessToken
    {
        public string CreateMeeting(
            string meetingNumber, string apiKey, string apiSecret, string userId, string username, bool canPublish = true, bool canSubscribe = true, bool canRecord = true)
        {
            var accessToken = new AccessToken(apiKey, apiSecret);
            
            var videoGrant = new VideoGrant
            {
                Room = meetingNumber,
                RoomCreate = true,
                RoomAdmin = true,
                RoomRecord = canRecord,
                CanPublish = canPublish,
                CanSubscribe = canSubscribe,
                CanPublishSources = new List<TrackSource>
                {
                    TrackSource.Camera,
                    TrackSource.Microphone,
                    TrackSource.ScreenShare,
                    TrackSource.ScreenShareAudio
                }
            };

            return accessToken.AddGrant(videoGrant)
                .SetIdentity(userId)
                .SetTTL(TimeSpan.FromHours(2))
                .SetName(username).ToJwt();
        }
        
        public string JoinMeeting(
            string meetingNumber, string apiKey, string apiSecret, string userId, string username, bool? canUpdateOwnMetadata = true)
        {
            var accessToken = new AccessToken(apiKey, apiSecret);
            
            var videoGrant = new VideoGrant
            {
              Room = meetingNumber,
              CanUpdateOwnMetadata = canUpdateOwnMetadata,
              RoomJoin = true
            };

            return accessToken.AddGrant(videoGrant)
                .SetIdentity(userId)
                .SetTTL(TimeSpan.FromHours(2))
                .SetName(username).ToJwt();
        }
        
        public string RecordMeeting(string meetingNumber, string apiKey, string apiSecret, string userId, string username)
        {
            var accessToken = new AccessToken(apiKey, apiSecret);
            
            var videoGrant = new VideoGrant
            {
                Room = meetingNumber, 
                RoomRecord = true,
                CanPublish = true,
                CanSubscribe = true,
                CanPublishSources = new List<TrackSource>
                {
                    TrackSource.Camera,
                    TrackSource.Microphone,
                    TrackSource.ScreenShare,
                    TrackSource.ScreenShareAudio
                }
            };

            return accessToken.AddGrant(videoGrant)
                .SetIdentity(userId)
                .SetTTL(TimeSpan.FromHours(2))
                .SetName(username).ToJwt();
        }
        
        public string GetAllMeeting(
            string meetingNumber, string apiKey, string apiSecret, string userId, string username)
        {
            var accessToken = new AccessToken(apiKey, apiSecret);
            
            var videoGrant = new VideoGrant
            {
                Room = meetingNumber, RoomList = true
            };

            return accessToken.AddGrant(videoGrant)
                .SetIdentity(userId)
                .SetTTL(TimeSpan.FromHours(2))
                .SetName(username).ToJwt();
        }
        
        public string GetMeetingInfoPermission(
            string meetingNumber, string apiKey, string apiSecret)
        {
            var accessToken = new AccessToken(apiKey, apiSecret);
            
            var videoGrant = new VideoGrant
            {
                Room = meetingNumber,
                CanSubscribe = true,
                RoomAdmin = true                      
            };

            return accessToken.AddGrant(videoGrant).SetTTL(TimeSpan.FromHours(2)).ToJwt();
        }
    }
}