import Avatar from 'boring-avatars'
import { useSession } from 'next-auth/react'
import { useState } from 'react'
import ProfileDropdown from '../ProfileDropdown/ProfileDropdown'

const ProfileButton = ({ size = 40 }) => {
  const { data: session } = useSession()
  const { email } = session!.user!

  const [attachTo, setAttachTo] = useState<HTMLElement | null>(null)
  const open = Boolean(attachTo)

  const handleOpen = (el: HTMLElement) => setAttachTo(el)
  const handleClose = () => setAttachTo(null)

  return (
    <>
      <div className={'cursor-pointer'} onClick={e => handleOpen(e.currentTarget)}>
        <Avatar
          size={size}
          name={email!}
          variant={'bauhaus'}
          colors={['#494949', '#D9D6D6', '#EF5450', '#00796B']}
        />
      </div>
      <ProfileDropdown attachTo={attachTo} open={open} handleClose={handleClose} />
    </>
  )
}

export default ProfileButton
