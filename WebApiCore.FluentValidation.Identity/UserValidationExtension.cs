using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace WebApiCore.FluentValidation.Identity
{
    public static class UserValidationExtension
    {
        /// <summary>
        /// Check that user exists in database if Id is presented
        /// </summary>
        public static IRuleBuilderOptions<T, string> UserExistsAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, UserManager<TUser> userManager)
            where TUser : IdentityUser
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    if (id == null)
                        return true;

                    var user = await userManager.FindByIdAsync(id)
                        .ConfigureAwait(false);

                    return user != null;
                })
                .WithMessage("User '{PropertyValue}' does not exist.");
        }

        /// <summary>
        /// Check user existance in database if Username is presented
        /// </summary>
        /// <param name="exists">Whether to check if exists or not exists</param>
        private static IRuleBuilderOptions<T, string> UsernameExistsAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, bool exists, UserManager<TUser> userManager)
            where TUser : IdentityUser
        {
            return ruleBuilder
                .MustAsync(async (username, cancellationToken) =>
                {
                    if (username == null)
                        return true;

                    var user = await userManager.FindByNameAsync(username)
                        .ConfigureAwait(false);

                    return (user != null) == exists;
                })
                .WithMessage("User '{PropertyValue}' does not exist.");
        }

        /// <summary>
        /// Check user existance in database if Username is presented
        /// </summary>
        public static IRuleBuilderOptions<T, string> UsernameExistsAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, UserManager<TUser> userManager) where TUser : IdentityUser
            => ruleBuilder.UsernameExistsAsync(true, userManager);

        /// <summary>
        /// Check user existance in database if Username is presented
        /// </summary>
        public static IRuleBuilderOptions<T, string> UsernameNotExistsAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, UserManager<TUser> userManager) where TUser : IdentityUser
            => ruleBuilder.UsernameExistsAsync(false, userManager);

        /// <summary>
        /// Check that user has role
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsInRoleAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, string role, UserManager<TUser> userManager)
            where TUser : IdentityUser
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    var user = await userManager.FindByIdAsync(id)
                        .ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    return await userManager.IsInRoleAsync(user, role)
                        .ConfigureAwait(false);
                })
                .WithMessage($"User '{{PropertyValue}}' must be in '{role}' role.");
        }

        /// <summary>
        /// Check that user has at least one role
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsInRoleAsync<T, TUser>(this IRuleBuilder<T, string> ruleBuilder, string[] roles, UserManager<TUser> userManager)
            where TUser : IdentityUser
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    var user = await userManager.FindByIdAsync(id)
                        .ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    var userRoles = await userManager.GetRolesAsync(user)
                        .ConfigureAwait(false);

                    return userRoles.Any(ur => roles.Contains(ur));
                })
                .WithMessage("User '{PropertyValue}' doesn't have required role.");
        }
    }
}
