import { Link } from "react-router-dom";
export default function NotFound() {
  return (
    <div className="bg-surface-dark">
      <div className="grid h-screen place-content-center px-4">
        <div className="text-center">
          <h1 className="text-9xl font-black text-gray-400">404</h1>
          <p className="text-2xl font-bold tracking-tight text-gray-900 sm:text-4xl">Uh-oh!</p>
          <p className="mt-4 text-surface-light">We can't find that page.</p>
          <Link to="dashboard/home" className="mt-6 inline-block rounded bg-secondary px-5 py-3 text-sm font-medium text-white hover:bg-primary focus:outline-none focus:ring">
      Go Back Home
    </Link>
        </div>
      </div>
    </div>
  );
}