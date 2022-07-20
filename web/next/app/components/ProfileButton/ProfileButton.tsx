import Avatar from 'boring-avatars'
import { useSession } from 'next-auth/react'

const ProfileButton = ({ size = 40 }) => {
  const { data: session } = useSession()
  const { email } = session!.user!

  return (
    <>
      <Avatar
        size={size}
        name={email!}
        variant={'bauhaus'}
        colors={['#494949', '#D9D6D6', '#EF5450', '#00796B']}
      />
    </>
  )
}

export default ProfileButton
