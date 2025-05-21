// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyAZOy2sYgigtusDGODYLyIX9JaS7ZoCpJ4",
  authDomain: "kcmart-5a8f1.firebaseapp.com",
  projectId: "kcmart-5a8f1",
  storageBucket: "kcmart-5a8f1.firebasestorage.app",
  messagingSenderId: "492132807423",
  appId: "1:492132807423:web:8a12fe8ff701718de092ca",
  measurementId: "G-FD9CEJFFW4"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
