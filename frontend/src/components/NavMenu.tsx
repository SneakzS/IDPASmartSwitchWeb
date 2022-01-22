import * as React from "react"
import { NavLink } from "react-router-dom"


interface NavMenuState {
    collapseNavMenu: boolean
}

export default function NavMenu({ }) {
    const [state, setState] = React.useState<NavMenuState>({ collapseNavMenu: true })

    const toggleNavMenu = () => setState(state => ({ ...state, collapseNavMenu: !state.collapseNavMenu }))
    const navMenuClass = state.collapseNavMenu ? "" : "";

    return <><div className="top-row pl-4 navbar navbar-dark" >
        <a className="navbar-brand" href="">SmartSwitchWeb</a>
        <button className="navbar-toggler" onClick={toggleNavMenu}>
            <span className="navbar-toggler-icon"></span>
        </button>
    </div>

        <div className={navMenuClass} onClick={toggleNavMenu}>
            <ul className="nav flex-column">
                <li className="nav-item px-3">
                    <NavLink className="nav-link" to="/">
                        <span className="oi oi-plus" aria-hidden="true"></span> Geräte
                    </NavLink>
                </li>
                <li className="nav-item px-3">
                    <NavLink className="nav-link" to="/fetchdata">
                        <span className="oi oi-list-rich" aria-hidden="true"></span> Fetch data
                    </NavLink>
                </li>
                <li className="nav-item px-3">
                    <NavLink className="nav-link" to="/settings">
                        <span className="oi oi-list-rich" aria-hidden="true"></span> settings
                    </NavLink>
                </li>
            </ul>
        </div></>
}