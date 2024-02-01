import { Routes, Route } from "react-router-dom";
import {
  Sidenav,
  DashboardNavbar
} from "@/widgets/layout";
import routes from "@/routes";

export function Dashboard() {

  return (
    <div className="min-h-screen bg-gradient-to-r from-surface-darkest to-[#262133]">
        <Sidenav
            brandName={"Ergo"}
            routes={routes}
            brandImg={"/img/logo.png"}
        />
      <div className="p-4 xl:ml-80">
        <DashboardNavbar />
        <Routes>
          {routes.map(
            ({ layout, pages }) =>
              layout === "dashboard" &&
              pages.map(({ path, element }) => (
                <Route exact path={path} element={element} />
              ))
          )}
        </Routes>
      </div>
    </div>
  );
}

Dashboard.displayName = "/src/layout/dashboard.jsx";
