import { Button } from '@mui/material'
import { signIn, useSession } from 'next-auth/react'
import ProfileButton from '../ProfileButton/ProfileButton'

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

  return <div>{isLoggedIn ? <ProfileButton /> : <SignInButton />}</div>
}

export default UserIcon
