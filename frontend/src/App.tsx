import React from 'react'
import { BrowserRouter, Routes, Route } from "react-router-dom"
import NavMenu from "./components/NavMenu"
import DevicePage from './components/DevicePage'
import WorkloadPage from './components/WorkloadPage'
import EventsPage from './components/EventsPage'


function App() {
    const reloadHandler: [() => void] = [() => { }]
    const onReload = (e: React.MouseEvent<HTMLAnchorElement>) => {
        e.preventDefault()
        reloadHandler[0]()
    }

    return (
        <BrowserRouter>
            <div className="page">
                <div className="sidebar">
                    <NavMenu />
                </div>

                <div className="main">
                    <div className="top-row px-4">
                        <a onClick={onReload} href="#">Reload</a>
                        <a href="https://docs.microsoft.com/aspnet/" target="_blank">About</a>
                    </div>

                    <div className="content px-4">
                        <Routes>
                            <Route path="/" element={<DevicePage reloadHandler={reloadHandler} />} />
                            <Route path="/workload/:clientGuid" element={<WorkloadPage reloadHandler={reloadHandler} />} />
                            <Route path="/events/:clientGuid" element={<EventsPage reloadHandler={reloadHandler} />} />
                        </Routes>
                    </div>
                </div>
            </div>
        </BrowserRouter>

    );
}

export default App;
