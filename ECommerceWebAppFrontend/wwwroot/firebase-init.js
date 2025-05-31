<!-- firebase-init.js -->
<script src="https://www.gstatic.com/firebasejs/9.23.0/firebase-app-compat.js"></script>
<script src="https://www.gstatic.com/firebasejs/9.23.0/firebase-auth-compat.js"></script>
<script src="https://www.gstatic.com/firebasejs/9.23.0/firebase-analytics-compat.js"></script>

<script>
  const firebaseConfig = {
    apiKey: "AIzaSyAZOy2sYgigtusDGODYLyIX9JaS7ZoCpJ4",
    authDomain: "kcmart-5a8f1.firebaseapp.com",
    projectId: "kcmart-5a8f1",
    storageBucket: "kcmart-5a8f1.appspot.com",
    messagingSenderId: "492132807423",
    appId: "1:492132807423:web:8a12fe8ff701718de092ca",
    measurementId: "G-FD9CEJFFW4"
  };

  firebase.initializeApp(firebaseConfig);
  firebase.analytics();
  const auth = firebase.auth();

  window.firebase = {
    signInWithEmailAndPassword: async function (email, password) {
      const userCredential = await auth.signInWithEmailAndPassword(email, password);
      const token = await userCredential.user.getIdToken();
      return token;
    },

    createUserWithEmailAndPassword: async function (email, password) {
      const userCredential = await auth.createUserWithEmailAndPassword(email, password);
      const token = await userCredential.user.getIdToken();
      return token;
    },

    signOut: async function () {
      await auth.signOut();
    }
  };
</script>
