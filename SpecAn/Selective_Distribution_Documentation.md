## Problem Statement

### Objective:
Enable automated distribution of specific subsets of the `instrument-foundation` repository into downstream repositories, ensuring each downstream repository contains only its relevant folders.

### Key Requirements:

#### Selective Distribution:
- Downstream repositories (e.g., `SpecAn`) must include only designated folders (e.g., `SpecAn`, `doc_SpecAn`) without unrelated content (e.g., `DcPwr`).

### Approaches Considered:

#### 1. Using GitHub Template:
- This approach involves creating a repository template on GitHub that includes only the designated folders for downstream repositories.
- Downstream repositories can be initialized directly from this template, ensuring only the relevant content is included.

#### 2. Git Submodules + Sparse Checkout:
- This method uses Git submodules to link the `instrument-foundation` repository to downstream repositories.
- Sparse checkout is configured to include only the designated folders (e.g., `SpecAn`, `doc_SpecAn`) while excluding unrelated content (e.g., `DcPwr`).

#### 3. Git Subtree:
- Git subtree is used to extract and distribute specific folders from the `instrument-foundation` repository into downstream repositories.
- This approach allows downstream repositories to maintain a copy of the designated folders without linking to the entire repository.

## Analysis of Solutions

We evaluated three standard Git strategies against the critical requirement:

### Option A: Git Submodules + Sparse Checkout

#### The Concept:
- Add `instrument-foundation` as a submodule and use Git's "Sparse Checkout" feature to hide unwanted folders locally.

#### Why it FAILS:
1. **Configuration is Local, Not Shared:**
   - The "Sparse Checkout" configuration (the list of folders to keep/hide) lives in the `.git/info/sparse-checkout` file on the developer's local machine.
2. **Updates Reflect on Submodule Repo:**
   - Changes made to the submodule will directly reflect in the submodule repository.
3. **Git Architecture Limitation:**
   - Git does not allow pushing this local configuration file to the remote server.
4. **The Result:**
   - When a teammate clones the repo, they download the submodule configuration, but they do not get the "hiding" rules. They will see all folders (the entire `instrument-foundation` repo) unless they manually run a setup script.

### Option B: Git Templates

- Git templates do not provide a direct way to selectively copy a subset of files.

### Option C: Git Subtree

#### Why it FAILS:

1. **The Conflict is Unavoidable:**
   - When using Git Subtree for selective folders, a technical conflict arises by manually deleting content from your repository that still exists in the source repository's history.
   - **Action:** You run `git subtree add`, then manually delete unwanted folders (e.g., `DcPwrTests`) and commit the deletion.
   - **Problem:** When you run `git subtree pull` to get updates, Git attempts to merge the entire upstream commit. Since the upstream still contains the files you deleted (and may have even modified them), the merge engine encounters a modify/delete conflict.

2. **The Maintenance Burden:**
   - The conflict cannot be skipped or automated away; you are forced to choose a side manually every time:
     - **Recurring Manual Fix:** The maintainer must stop the merge, manually run `git rm` on the conflicted files again, and then commit the merge result.
     - **Result:** This continuous cycle of Pull → Conflict → Manually Re-Delete → Commit eliminates the simplicity gained by using Subtree and significantly increases your workload during every update, proving it doesn't work directly or easily for maintenance.