import {Route, Routes} from "react-router";
import Main from "./screens/main/Main.tsx";

function App() {

  return (
      <>
        <Routes>
            <Route path="/">
                <Route index element={<Main/>}></Route>
            </Route>
        </Routes>
      </>
  );
}

export default App;