import Video from 'twilio-video';
import axios from 'axios';
import React from "react";
import {useState, useEffect} from "react";

const TwilioVideo = () => {
    // const [token, setToken] = useState('');
    //
    // useEffect(() => {
    //     const fetchToken = async () => {
    //         try {
    //             const response = await axios.get('http://localhost:8003/Chat/get-access-token');
    //             setToken(response.data);
    //             console.log(token);
    //         } catch (error) {
    //             console.error('Error fetching token:', error);
    //         }
    //     };
    //
    //     fetchToken();
    //    
    //     // const x = joinVideoRoom("room-1", token);
    // }, []);
    //
    // const joinVideoRoom = async (roomName, token) => {
    //     const room = await Video.connect(token, {
    //         room: roomName,
    //     });
    //     return room;
    // };

    const { connect } = require('twilio-video');

    connect('eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6InR3aWxpby1mcGE7dj0xIn0.eyJpc3MiOiJTS2Q1ZDRiMGUzNjQyNDU0MjAzZmQyMDNiMzZlYmVkNWZmIiwiZXhwIjoxNzAwMzk2NTcxLCJqdGkiOiJTS2Q1ZDRiMGUzNjQyNDU0MjAzZmQyMDNiMzZlYmVkNWZmLTE3MDAzOTI5NzEiLCJzdWIiOiJBQzRhMDlmYzBhZjk5NWYzMTE0MjJiMmM4OWY0MjI2OTZlIiwiZ3JhbnRzIjp7ImlkZW50aXR5IjoidXNlciIsInZpZGVvIjp7InJvb20iOiJyb29tLTEifX19.2R86gz64aUx2rrckOKPJgedYc9u1Z1UNgZWz0eeNQJE', 
        { name:'room-1' }).then(room => {
        console.log(`Successfully joined a Room: ${room}`);
        room.on('participantConnected', participant => {
            console.log(`A remote Participant connected: ${participant}`);
        });
    }, error => {
        console.error(`Unable to connect to Room: ${error.message}`);
    });

    return (
        <div>
            <h1>Twilio Video Token</h1>
        </div>
    );
};

export default TwilioVideo;