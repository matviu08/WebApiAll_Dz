import {Route, Routes} from "react-router";
import Main from "./screens/main/Main.tsx";
import Header from "./components/Header.tsx";
import Login from "./screens/auth/Login.tsx";
import Register from "./screens/auth/Register.tsx";
import Profile from "./screens/profile/Profile.tsx";

function App() {

  return (
      <>
          <Header/>
          <Routes>
              <Route path="/">
                  <Route index element={<Main/>}></Route>
                  <Route path="/login" element={<Login/>}></Route>
                  <Route path="/register" element={<Register/>}></Route>
                  <Route path="/profile" element={<Profile/>}></Route>
              </Route>
          </Routes>
      </>
  );
}

export default App;