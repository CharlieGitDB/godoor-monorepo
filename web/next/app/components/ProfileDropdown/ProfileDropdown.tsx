import { Menu, MenuItem } from '@mui/material'
import { signOut } from 'next-auth/react'

type Props = {
  attachTo: HTMLElement | null
  open: boolean
  handleClose: () => void
}

export const ProfileDropdown = ({ attachTo, open, handleClose }: Props) => (
  <Menu
    open={open}
    anchorEl={attachTo}
    onClose={() => handleClose()}
    disableAutoFocusItem
  >
    <MenuItem>Placeholder</MenuItem>
    <MenuItem onClick={() => signOut()}>Logout</MenuItem>
  </Menu>
)

export default ProfileDropdown
