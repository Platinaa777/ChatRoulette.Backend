import Video from 'twilio-video';
import React from "react";
import {useState} from "react";

const TwilioVideo = () => {
    const [roomName, setRoomName] = useState('');
    const [token, setToken] = useState('');
    const [videoRoom, setVideoRoom] = useState(null);

    useEffect(() => {
        // Функция для получения токена и имени комнаты от сервера
        const fetchTokenAndRoomName = async () => {
            try {
                const response = await fetch('your-backend-url-to-get-token-and-room');
                const data = await response.json();
                setToken(data.token);
                setRoomName(data.roomName);
            } catch (error) {
                console.error('Error fetching token and room name:', error);
            }
        };

        fetchTokenAndRoomName();
    }, []);

    const joinVideoRoom = async () => {
        try {
            const videoRoom = await Video.connect(token, {
                name: roomName
            });
            setVideoRoom(videoRoom);
        } catch (error) {
            console.error('Error connecting to video room:', error);
        }
    };

    return (
        <div>
            <h1>Twilio Video Component</h1>
            <button onClick={joinVideoRoom}>Join Video Room</button>
            {/* Видео-компоненты для отображения видео-потока */}
            {videoRoom && (
                <div>
                    {videoRoom.localParticipant.videoTracks.map(track => (
                        <video key={track.sid} ref={ref => track.attach(ref)} />
                    ))}
                    {videoRoom.participants.map(participant => (
                        <div key={participant.sid}>
                            {participant.videoTracks.map(track => (
                                <video key={track.sid} ref={ref => track.attach(ref)} />
                            ))}
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default TwilioVideo;