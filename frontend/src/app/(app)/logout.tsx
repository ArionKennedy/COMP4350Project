import { useEffect } from "react";

import { useSession } from "@/auth-ctx";

export default function LogoutScreen() {
  const { signOut } = useSession();

  useEffect(() => {
    signOut();
  }, [signOut]);

  return null;
}
