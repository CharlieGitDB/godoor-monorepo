import { AppBar, Toolbar, Typography } from '@mui/material'
import Link from 'next/link'
import UserIcon from '../UserIcon/UserIcon'

const Header = () => (
  <AppBar position={'static'}>
    <Toolbar>
      <Typography variant={'h6'} component={'div'} sx={{ flexGrow: 1 }}>
        <Link href="/" passHref>
          <span className={'cursor-pointer'}>Godoor</span>
        </Link>
      </Typography>
      <UserIcon />
    </Toolbar>
  </AppBar>
)

export default Header
