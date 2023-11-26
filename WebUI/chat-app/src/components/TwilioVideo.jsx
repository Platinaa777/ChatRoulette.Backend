import Video, {connect, createLocalVideoTrack} from 'twilio-video';
import axios from 'axios';
import React from "react";
import {useState, useEffect} from "react";
import '../styles/TwilioVideo.css'

const TwilioVideo = () => {
    const { connect } = require('twilio-video');
    const [token, setToken] = useState('');
    // const [localVideoTrack, setLocalVideoTrack] = useState(null);
    //
    // useEffect(() => {
    //     // Получаем доступ к медиапотокам браузера
    //     navigator.mediaDevices.getUserMedia({ video: true, audio: true })
    //         .then(mediaStream => {
    //             // Создаем видео трек из медиапотока
    //             const videoTrack = new Video.LocalVideoTrack(mediaStream.getVideoTracks()[0]);
    //             setLocalVideoTrack(videoTrack);
    //         })
    //         .catch(error => {
    //             console.error('Error accessing media devices:', error);
    //         });
    // }, []);

    const ShowVideo = () => {
        
        createLocalVideoTrack().then(track => {
            const localMediaContainer = document.getElementById('local-media');
            localMediaContainer.appendChild(track.attach());
        });
    }
    
    const ConnectTo = async () => {
        
        const response = await fetch(`http://localhost:8003/Chat/find-room`);
        
        if (!response.ok) {
            console.error(`Unable to connect to Room: ${await response.json()}`);
        }
        
        const jsonData = await response.json()
        
        if (!jsonData.isValid) {
            console.error(jsonData.message);
        }
        
        setToken(jsonData.accessToken);
        console.log(token);
        console.log(jsonData);
        console.log(jsonData.roomName);
        
        connect(jsonData.accessToken, { name: jsonData.roomName })
            .then(room => {
                console.log(`Successfully joined a Room: ${room}`);
                ShowVideo();
                room.on('participantConnected', participant => {
                    console.log(`A remote Participant connected: ${participant}`);
                });
            }, error => {
            console.error(`Unable to connect to Room123 : ${error.message}`);
        });
    }
   
    return (
        <div>
            <h1>Twilio Video Token</h1>
            
            <h1>
                <button onClick={() => ConnectTo()}>Connect to Room</button>
            </h1>
            {/*<div id="video-container">*/}
            {/*    {localVideoTrack && <video ref={ref => localVideoTrack.attach(ref)} autoPlay={true} controls={true}></video>}*/}
            {/*</div>*/}
            <div id="local-media">
                
            </div>
        </div>
    );
};

export default TwilioVideo;