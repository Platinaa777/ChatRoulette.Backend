import Video, {connect, createLocalVideoTrack} from 'twilio-video';
import axios from 'axios';
import React from "react";
import {useState, useEffect} from "react";
import '../styles/TwilioVideo.css'

const TwilioVideo = () => {
    const { connect } = require('twilio-video');
    const [token, setToken] = useState('');
    const [room, setRoom] = useState('');
    const [localVideoTrack, setLocalVideoTrack] = useState(null);

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
        
    const CreateRoom = async () => {
        const response = axios.get('http://localhost:8003/Chat/create-room')
            .then(x => {
                console.log(x.data)
                setRoom(x.data.room)
            });
    }
    
    const GetAccessToken = async (room) => {
        try {
            const response = 
                await axios.get(`http://localhost:8003/Chat/get-access-token/${room}`);
            setToken(response.data);
            console.log(token);
        } catch (error) {
            console.error('Error fetching token:', error);
        }
    }
   
    const ConnectTo = (token, room) => {
        connect(token,
            { name: room }).then(room => {
            console.log(`Successfully joined a Room: ${room}`);
            room.on('participantConnected', participant => {
                console.log(`A remote Participant connected: ${participant}`);
            });
        }, error => {
            console.error(`Unable to connect to Room: ${error.message}`);
        });
    }
   
    return (
        <div>
            <h1>Twilio Video Token</h1>
            <h1>
                <button onClick={() => CreateRoom()}>Create room</button>
            </h1>
            <h1>
                <button onClick={() => GetAccessToken()}>
                    Get token to special room
                </button>
            </h1>
            <h1>
                <input value={room} onChange={(e) => setRoom(e.target.value)}/>
            </h1>
            <h1>
                <button onClick={() => ConnectTo(token, room)}>подключиться в комнату</button>
            </h1>
            <h1>
                <button onClick={() => ShowVideo()}>Show room</button>
            </h1>
            {
                room === '' ? 
                    ""
                    :
                    <div>Name of room: {room}</div>
            }
            <h1>
                <div>
                {
                    token === '' ?
                        "" 
                        :
                        <div>
                            <div>access token to room: {room}</div>
                            <div >jwt token - </div>
                            <pre style={{ whiteSpace: 'pre-wrap', wordWrap: 'break-word' }}>{token}</pre>
                        </div>
                }
                </div>
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