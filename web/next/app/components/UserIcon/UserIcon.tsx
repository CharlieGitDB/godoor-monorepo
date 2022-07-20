import { Button } from '@mui/material'
import { signIn, useSession } from 'next-auth/react'

const UserIcon = () => {
  const { data: session } = useSession()

  const isLoggedIn = !!session?.user

  const SignInButton = () => (
    <Button
      onClick={() => signIn('azure-ad-b2c')}
      variant={'contained'}
      color={'secondary'}
    >
      Sign In
    </Button>
  )

  const Profile = () => <>{JSON.stringify(session?.user)}</>

  return <div>{isLoggedIn ? <Profile /> : <SignInButton />}</div>
}

export default UserIcon
