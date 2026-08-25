import {Route, Routes} from "react-router";
import Main from "./screens/main/Main.tsx";
import Header from "./components/Header.tsx";
import Login from "./screens/auth/Login.tsx";
import Register from "./screens/auth/Register.tsx";

function App() {

  return (
      <>
          <Header/>
          <Routes>
              <Route path="/">
                  <Route index element={<Main/>}></Route>
              </Route>
              <Route path="/login">
                  <Route index element={<Login/>}></Route>
              </Route>
              <Route path="/register">
                  <Route index element={<Register/>}></Route>
              </Route>
          </Routes>
      </>
  );
}

export default App;