importScripts('https://www.gstatic.com/firebasejs/8.0.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.0.1/firebase-messaging.js');


var firebaseConfig = {
    apiKey: "AIzaSyCyTYYYhDbzecUQjBVlac3rQtdHbCFeRqo",
    authDomain: "codeight-7b086.firebaseapp.com",
    projectId: "codeight-7b086",
    storageBucket: "codeight-7b086.appspot.com",
    messagingSenderId: "687174581243",
    appId: "1:687174581243:web:53c5391ec5be9fb03ba95e"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

// Retrieve Firebase Messaging object.
const messaging = firebase.messaging();